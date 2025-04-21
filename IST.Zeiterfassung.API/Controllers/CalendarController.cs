using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.DTOs.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;

    public CalendarController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    /// <summary>
    /// Gibt alle Kalendertage eines Monats für den aktuellen Benutzer zurück.
    /// </summary>
    /// <param name="month">Monatswert im Format YYYY-MM (z. B. 2025-05)</param>
    /// <returns>Liste von Kalendertagen mit Zeitbuchungen, Abwesenheiten und Feiertagen</returns>
    /// <response code="200">Erfolgreich</response>
    /// <response code="400">Ungültiger Parameter</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(List<CalendarDayDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMonthlyCalendar([FromQuery] string month)
    {
        if (!DateOnly.TryParse($"{month}-01", out _))
            return BadRequest("Ungültiges Monatsformat. Verwende z. B. 2025-05");

        var userId = GetCurrentUserId();

        var result = await _calendarService.GetMonthlyCalendarAsync(userId, month);

        return Ok(result);
    }

    /// <summary>
    /// Gibt alle Kalendertage eines Monats für einen bestimmten Benutzer zurück (nur Admin).
    /// </summary>
    /// <param name="userId">Benutzer-ID</param>
    /// <param name="month">Monatswert im Format YYYY-MM (z. B. 2025-05)</param>
    /// <returns>Liste von Kalendertagen mit Zeitbuchungen, Abwesenheiten und Feiertagen</returns>
    /// <response code="200">Erfolgreich</response>
    /// <response code="400">Ungültiger Parameter</response>
    [HttpGet("{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<CalendarDayDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMonthlyCalendarForUser(Guid userId, [FromQuery] string month)
    {
        if (!DateOnly.TryParse($"{month}-01", out _))
            return BadRequest("Ungültiges Monatsformat. Verwende z. B. 2025-05");

        var result = await _calendarService.GetMonthlyCalendarAsync(userId, month);
        return Ok(result);
    }



    private Guid GetCurrentUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
