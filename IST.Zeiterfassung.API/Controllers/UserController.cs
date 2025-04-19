using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Registriert einen neuen Benutzer.
    /// </summary>
    /// <param name="dto">Daten für Registrierung</param>
    /// <returns>Registrierter Benutzer</returns>
    /// <response code="201">Benutzer erfolgreich registriert</response>
    /// <response code="400">Eingabefehler</response>
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
    {
        var result = await _userService.RegisterAsync(dto);
        return CreatedAtAction(nameof(Register), new { id = result.Value?.Id }, result.Value);
    }

    /// <summary>
    /// Gibt alle Benutzer zurück (nur zu Testzwecken).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Gibt ein Benutzerprofil anhand der ID zurück.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(new { message = result.ErrorMessage });

        return Ok(result.Value);
    }

    /// <summary>
    /// Ändert das Passwort eines Benutzers.
    /// </summary>
    [HttpPut("{id:guid}/password")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDTO dto)
    {
        var result = await _userService.ChangePasswordAsync(id, dto);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = result.Value });
    }

    /// <summary>
    /// Ändert die Rolle eines Benutzers (z. B. Admin oder Employee).
    /// </summary>
    [HttpPut("{id:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeUserRoleDTO dto)
    {
        var result = await _userService.ChangeRoleAsync(id, dto);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = result.Value });
    }

    /// <summary>
    /// Schaltet den Aktiv-Status eines Benutzers um (aktiv/inaktiv).
    /// </summary>
    [HttpPut("{id:guid}/toggle-aktiv")]
    public async Task<IActionResult> ToggleAktiv(Guid id)
    {
        var result = await _userService.ToggleAktivAsync(id);

        if (!result.Success)
            return NotFound(new { message = result.ErrorMessage });

        return Ok(new { message = result.Value });
    }


}
