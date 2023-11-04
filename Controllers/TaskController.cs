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
    private readonly IConfiguration _configuration;

    public TaskController(AuthenticationService authService, UserRepository userRepository, TaskRepository taskRepository, IConfiguration configuration)
    {
        _authService = authService;
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _configuration = configuration;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tasks>?>> GetTasks()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);
        
        var tasks = _taskRepository.GetAllUserTasksAsync(username);
        return await tasks;
    }
    [HttpPost("new")]
    public async Task<IActionResult> Post([FromBody] Tasks Task)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);

        Task.Owner = username;

        await _taskRepository.Add(Task);

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