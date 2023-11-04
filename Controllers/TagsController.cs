// Path: Controllers/TagsController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Data;

[Authorize]
[Route("[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly AuthenticationService _authService;
    private readonly TagsRepository _tagsRepository;


    public TagsController(ApplicationDbContext context, AuthenticationService authService, TagsRepository tagsRepository)
    {
        _authService = authService;
        _context = context;
        _tagsRepository = tagsRepository;
    }
    [HttpGet]
    public ActionResult<IEnumerable<County>> GetCounties()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);

        var counties = _tagsRepository.GetTagsList(username);
        return Ok(counties);
    }

    [HttpPost("new")]
    public async Task<ActionResult<Tags>> PostTag(Tags tag)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);
        var id = await _tagsRepository.GetNextId(username);
        tag.Id = id;
        tag.Owner = username;

        await _tagsRepository.CreateTag(tag);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTag(int id, Tags tag)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);

        if (id != tag.Id || username != tag.Owner)
        {
            return BadRequest();
        }

        _context.Entry(tag).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);

        var tag = await _context.Tags.FindAsync(id, username);
        if (tag == null)
        {
            return NotFound();
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}