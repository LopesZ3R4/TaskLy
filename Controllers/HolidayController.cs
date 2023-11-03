// Path: c:\TaskLy\Controllers\HolidayController.cs
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class HolidayController : ControllerBase
{
    private readonly HolidayService _holidayService;
    private readonly HolidayRepository _holidayRepository;

    public HolidayController(HolidayService holidayService, HolidayRepository holidayRepository)
    {
        _holidayService = holidayService;
        _holidayRepository = holidayRepository;
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncHolidays()
    {
        await _holidayService.ExecuteAsync();
        return Ok();
    }

    [HttpGet]
    public IActionResult GetHolidays()
    {
        var holidays = _holidayRepository.GetAll();
        return Ok(holidays);
    }
}