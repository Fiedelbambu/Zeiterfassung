using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IST.Zeiterfassung.Application.DTOs.Absence;
using IST.Zeiterfassung.Application.Interfaces;
using System.Security.Claims;

namespace IST.Zeiterfassung.API.Controllers;

/// <summary>
/// Stellt Funktionen zur Verwaltung von Abwesenheiten bereit (z. B. Urlaub, Krankheit, Home Office).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AbsenceController : ControllerBase
{
    private readonly IAbsenceService _service;

    public AbsenceController(IAbsenceService service)
    {
        _service = service;
    }

    /// <summary>
    /// Beantragt eine neue Abwesenheit für den aktuellen Benutzer.
    /// </summary>
    /// <param name="dto">Die Abwesenheitsdaten (Start, Ende, Typ, Kommentar)</param>
    /// <returns>Die ID der erstellten Abwesenheit</returns>
    /// <response code="201">Abwesenheit wurde erfolgreich erstellt</response>
    /// <response code="400">Ungültige Eingabedaten</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateAbsenceDTO dto)
    {
        var userId = UserId();
        var result = await _service.CreateAsync(dto, userId);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Gibt alle Abwesenheiten des aktuellen Benutzers zurück.
    /// </summary>
    /// <returns>Liste von Abwesenheiten</returns>
    [HttpGet("me")]
    [ProducesResponseType(typeof(List<AbsenceResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwn()
    {
        var userId = UserId();
        var result = await _service.GetAllForUserAsync(userId);
        return Ok(result.Value);
    }

    /// <summary>
    /// Gibt eine einzelne Abwesenheit anhand der ID zurück.
    /// </summary>
    /// <param name="id">Die ID der Abwesenheit</param>
    /// <returns>Die Abwesenheit oder ein Fehler</returns>
    /// <response code="200">Erfolgreich gefunden</response>
    /// <response code="404">Nicht gefunden</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AbsenceResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.Success ? Ok(result.Value) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Löscht eine beantragte Abwesenheit des aktuellen Benutzers.
    /// </summary>
    /// <param name="id">Die ID der zu löschenden Abwesenheit</param>
    /// <returns>Statusmeldung</returns>
    /// <response code="200">Erfolgreich gelöscht</response>
    /// <response code="403">Nicht berechtigt</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = UserId();
        var result = await _service.DeleteAsync(id, userId);
        return result.Success ? Ok() : Forbid(result.ErrorMessage);
    }

    /// <summary>
    /// Gibt alle Abwesenheiten im System zurück (nur Admins).
    /// </summary>
    /// <returns>Liste aller Abwesenheiten</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<AbsenceResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    /// <summary>
    /// Genehmigt einen Abwesenheitsantrag (nur Admins).
    /// </summary>
    /// <param name="id">Die ID des zu genehmigenden Antrags</param>
    /// <returns>Statusmeldung</returns>
    /// <response code="200">Erfolgreich genehmigt</response>
    /// <response code="400">Fehlerhafte Anfrage</response>
    [HttpPatch("{id}/approve")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _service.ApproveAsync(id);
        return result.Success ? Ok() : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Lehnt einen Abwesenheitsantrag ab (nur Admins).
    /// </summary>
    /// <param name="id">Die ID des zu ablehnenden Antrags</param>
    /// <returns>Statusmeldung</returns>
    /// <response code="200">Erfolgreich abgelehnt</response>
    /// <response code="400">Fehlerhafte Anfrage</response>
    [HttpPatch("{id}/reject")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reject(Guid id)
    {
        var result = await _service.RejectAsync(id);
        return result.Success ? Ok() : BadRequest(result.ErrorMessage);
    }
    //Todo: in allen Controller-Methoden die Fehlerbehandlung anpassen, um den Statuscode 500 zu vermeiden und stattdessen spezifische Fehlercodes zurückzugeben.
    /// <summary>
    /// Liefert die Benutzer-ID aus dem JWT-Token.
    /// </summary>
    private Guid UserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
