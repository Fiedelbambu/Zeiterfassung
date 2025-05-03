using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.DTOs.Settings;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Swashbuckle.AspNetCore.Annotations;

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

    /// <summary>
    /// Ruft die aktuellen Systemeinstellungen ab.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(SystemSettingsDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<SystemSettingsDTO>> Get()
    {
        var settings = await _service.GetSettingsAsync();
        return Ok(settings);
    }

    /// <summary>
    /// Aktualisiert die Systemeinstellungen.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Put(SystemSettingsDTO dto)
    {
        await _service.UpdateSettingsAsync(dto);
        return NoContent();
    }

    /// <summary>
    /// Prüft die Authentifizierung und gibt User-Claims zurück (Debug-Zwecke).
    /// </summary>
    [HttpGet("test-auth")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Lädt ein neues Hintergrundbild hoch und ersetzt das alte Bild.
    /// </summary>
    /// <remarks>
    /// Unterstützte Formate: JPG, PNG, WEBP.
    /// Alte Bilder im Upload-Verzeichnis werden beim Hochladen gelöscht.
    /// </remarks>
    [HttpPost("upload-background")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadBackground(IFormFile file)
    {
        try
        {
            var url = await _service.UploadBackgroundImageAsync(file);
            return Ok(new { url });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
