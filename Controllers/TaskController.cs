// .\Controllers\TaskController.cs
using Microsoft.AspNetCore.Mvc;
using Services;
using Data;
using Utils;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly AuthenticationService _authService;
    private readonly UserRepository _userRepository;
    private readonly TaskRepository _taskRepository;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TaskController(AuthenticationService authService, UserRepository userRepository, TaskRepository taskRepository, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _authService = authService;
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
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
    public async Task<IActionResult> DeleteTask(long id)
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
}