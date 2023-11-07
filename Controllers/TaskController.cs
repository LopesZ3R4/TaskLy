// .\Controllers\TaskController.cs
using Microsoft.AspNetCore.Mvc;
using Services;
using Data;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly AuthenticationService _authService;
    private readonly UserRepository _userRepository;
    private readonly TaskRepository _taskRepository;
    private readonly TagsRepository _tagsRepository;
    private readonly IConfiguration _configuration;

    public TaskController(AuthenticationService authService, UserRepository userRepository, TaskRepository taskRepository, IConfiguration configuration, TagsRepository tagsRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _tagsRepository = tagsRepository;
        _configuration = configuration;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>?>> GetTasks()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);
        
        var tasks = _taskRepository.GetAllUserTasks(username);
        return await tasks;
    }
    [HttpPost("new")]
    public async Task<IActionResult> Post([FromBody] TaskDto taskDto)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);
        taskDto.Owner = username;

        foreach (var tag in taskDto.Tags)
        {
            var existingTag =_tagsRepository.Exists(tag.Id,username);
            if (!existingTag)
            {
                return BadRequest($"Tag with id {tag.Id} does not exist.");
            }
        }
        await _taskRepository.Add(taskDto);

        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _taskRepository.GetTaskbyID(id);

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);
        if (task == null)
        {
            return NotFound();
        }
        if (task.Owner != username)
        {
            return Unauthorized();
        }

        await _taskRepository.Delete(task);
        return NoContent();
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTask(int Id, [FromBody] Tasks updatedTask)
    {
        var task = await _taskRepository.GetTaskbyID(Id);

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);
        if (task == null)
        {
            return NotFound();
        }
        if (task.Owner != username)
        {
            return Unauthorized();
        }
        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.StartDate = updatedTask.StartDate;
        task.Duration = updatedTask.Duration;
        task.AutoFinish = updatedTask.AutoFinish;
        task.Status = updatedTask.Status;

        await _taskRepository.Update(task);
        return NoContent();
    }
}