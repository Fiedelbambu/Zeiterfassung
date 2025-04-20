using IST.Zeiterfassung.Application.DTOs.Zeitmodell;
using IST.Zeiterfassung.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimeModelController : ControllerBase
{
    private readonly ITimeModelService _service;

    public TimeModelController(ITimeModelService service)
    {
        _service = service;
    }

    /// <summary>
    /// Gibt das Zeitmodell des angemeldeten Benutzers zurück.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _service.GetByUserIdAsync(userId);

        if (!result.Success)
            return NotFound(new { message = result.ErrorMessage });

        return Ok(result.Value);
    }

    /// <summary>
    /// Erstellt oder aktualisiert das Zeitmodell für den angemeldeten Benutzer.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] ZeitmodellDTO dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _service.SetOrUpdateAsync(userId, dto);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = result.Value });
    }

    /// <summary>
    /// Berechnet den Monats-Saldo laut Zeitmodell.
    /// </summary>
    [HttpGet("saldo")]
    public async Task<IActionResult> GetSaldo([FromQuery] int jahr, [FromQuery] int monat)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _service.BerechneMonatssaldoAsync(userId, jahr, monat);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(result.Value);
    }
}
