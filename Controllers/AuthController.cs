// .\Controllers\AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Services;
using Data;
using Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService _authService;
    private readonly UserRepository _userRepository;
    private readonly CountyRepository _countyRepository;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthController(AuthenticationService authService, UserRepository userRepository, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, CountyRepository countyRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
        _countyRepository = countyRepository;
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var isValid = _authService.ValidateCredentials(request.Username, request.Password);
        if (!isValid)
        {
            Console.WriteLine("User does not have valid credentials");
            return Unauthorized();
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken
        (
            issuer: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var user = _userRepository.Get(request.Username);

        user.SetToken(tokenString);
        _userRepository.Update(user);

        _ = Task.Run(async () =>
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var taskRepository = scope.ServiceProvider.GetRequiredService<TaskRepository>();
                try
                {
                    List<Tasks>? pendingTasks = await taskRepository.GetUserPendingTasks(request.Username);
                    taskRepository.FinnishTasks(pendingTasks);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        });
        return Ok(new { Token = tokenString });
    }

    [HttpPost("register")]
    [SwaggerResponse(StatusCodes.Status200OK, "User registered successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid email format or the provided county does not exist", typeof(string))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "A user with the same username or email already exists", typeof(string))]
    public IActionResult Register([FromBody] User newUser)
    {
        if (!EmailValidator.IsValidEmail(newUser.GetEmail()))
        {
            return BadRequest("Invalid email format");
        }
        var existingUser = _userRepository.Exists(newUser.GetUsername(), newUser.GetEmail());

        if (existingUser)
        {
            return Conflict("A user with the same username or email already exists");
        }
        var existingCounty = _countyRepository.Exists(newUser.GetCountyCode());

        if (!existingCounty)
        {
            return BadRequest("The provided county does not exist");
        }

        newUser.SetPassword(_authService.HashPassword(newUser.GetPassword()));
        _userRepository.Add(newUser);

        return Ok();
    }

}