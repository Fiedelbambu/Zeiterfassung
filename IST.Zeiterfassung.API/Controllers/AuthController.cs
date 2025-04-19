using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Authentifiziert einen Benutzer.
    /// </summary>
    /// <param name="dto">Login-Daten</param>
    /// <returns>Benutzerdaten oder Fehlermeldung</returns>
    /// <response code="200">Login erfolgreich</response>
    /// <response code="400">Benutzername oder Passwort falsch</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
    {
        var result = await _userService.LoginAsync(dto);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(result.Value);
    }
}
