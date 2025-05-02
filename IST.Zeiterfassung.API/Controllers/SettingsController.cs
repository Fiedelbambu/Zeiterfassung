using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.DTOs.Settings;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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

    [HttpGet("test-auth")]
    public IActionResult TestAuth()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

        return Ok(new
        {
            userId,
            role,
            email
        });
    }
}
