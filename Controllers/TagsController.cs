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
    public async Task<ActionResult<Tags>> CreateTag(NewTagDto newTagDto)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);
        var id = await _tagsRepository.GetNextId(username);
        
        Tags Tag = newTagDto.ToModel(id,username);

        await _tagsRepository.CreateTag(Tag);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(int id, Tags tag)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var username = _authService.GetUsernameFromToken(token);

        if (id != tag.GetId() || username != tag.GetOwner())
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