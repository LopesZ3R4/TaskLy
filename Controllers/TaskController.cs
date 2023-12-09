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
        taskDto.SetOwner(username);

        foreach (var tag in taskDto.GetTags())
        {
            var tagid = tag.GetId();
            var existingTag =_tagsRepository.Exists(tagid,username!);
            if (!existingTag)
            {
                return BadRequest($"Tag with id {tagid} does not exist.");
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
        if (task.GetOwner() != username)
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
        if (task.GetOwner() != username)
        {
            return Unauthorized();
        }
        task.SetTitle(updatedTask.GetTitle());
        task.SetDescription(updatedTask.GetDescription());
        task.SetStartDate(updatedTask.GetStartDate());
        task.SetDuration(updatedTask.GetDuration());
        task.SetAutoFinish(updatedTask.isAutoFinish());
        task.SetStatus(updatedTask.GetStatus());

        await _taskRepository.Update(task);
        return NoContent();
    }
}