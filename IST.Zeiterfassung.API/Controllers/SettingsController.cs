using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.DTOs.Settings;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize(Roles = "Admin")]
public class SettingsController : ControllerBase
{
    private readonly ISystemSettingsService _service;

    public SettingsController(ISystemSettingsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<SystemSettingsDTO>> Get()
    {
        var settings = await _service.GetSettingsAsync();
        return Ok(settings);
    }

    [HttpPut]
    public async Task<IActionResult> Put(SystemSettingsDTO dto)
    {
        await _service.UpdateSettingsAsync(dto);
        return NoContent();
    }
}
