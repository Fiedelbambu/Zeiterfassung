using IST.Zeiterfassung.Application.DTOs.Settings;
using IST.Zeiterfassung.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly ISettingsService _settingsService;

    public AdminController(IAdminService adminService, ISettingsService settingsService)
    {
        _adminService = adminService;
        _settingsService = settingsService;
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        var status = await _adminService.GetSystemStatusAsync();
        return Ok(status);
    }

    [HttpPut("settings")]
    public IActionResult SaveSettings([FromBody] SystemSettingsDTO dto)
    {
        _settingsService.Save(dto);
        return Ok(new { message = "Einstellungen gespeichert." });
    }

    [HttpGet("settings")]
    public IActionResult GetSettings()
    {
        var settings = _settingsService.Load();
        return Ok(settings);
    }



}
