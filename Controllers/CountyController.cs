// File: c:\TaskLy\Controllers\CountyController.cs
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class CountyController : ControllerBase
{
    private readonly CountyRepository _countyRepository;

    public CountyController(CountyRepository countyRepository)
    {
        _countyRepository = countyRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<County>>> GetCounties()
    {
        var counties = await _countyRepository.GetAllCounties();
        return Ok(counties);
    }
}