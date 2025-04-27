using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using IST.Zeiterfassung.Application.DTOs.Auth;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly ILoginAuditRepository _loginAuditRepository;

    public AuthController(
        IUserService userService,
        ITokenService tokenService,
        IUserRepository userRepository,
        ILoginAuditRepository loginAuditRepository)
    {
        _userService = userService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _loginAuditRepository = loginAuditRepository;
    }

    /// <summary>
    /// Authentifiziert einen Benutzer per Benutzername + Passwort.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
    {
        var result = await _userService.LoginAsync(dto, HttpContext);

        // Audit-Datensatz vorbereiten
        var audit = new LoginAudit
        {
            LoginMethod = LoginMethod.Passwort,
            Erfolgreich = result.Success,
            IPAdresse = HttpContext.Connection.RemoteIpAddress?.ToString(),
            Ort = "Web-Login",
            UserId = result.Success ? result.Value!.Id : null
        };
        await _loginAuditRepository.AddAsync(audit);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        var token = _tokenService.CreateToken(result.Value!);

        var dtoResult = new UserResponseDTO
        {
            Id = result.Value!.Id,
            Username = result.Value.Username,
            Email = result.Value.Email,
            Role = result.Value.Role.ToString(),
          
        };

        return Ok(new
        {
            token,
            user = dtoResult
        });
    }

    /// <summary>
    /// Gibt die Informationen aus dem JWT zurück.
    /// </summary>
    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Unbekannt";
        var username = User.FindFirstValue(ClaimTypes.Name) ?? "Unbekannt";
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "Unbekannt";

        return Ok(new
        {
            id = userId,
            username,
            role
        });
    }

    /// <summary>
    /// Login per NFC UID (z. B. Terminalscan).
    /// </summary>
    [HttpPost("nfc")]
    public async Task<IActionResult> NfcLogin([FromBody] NfcLoginDTO dto)
    {
        var result = await _userService.LoginByNfcAsync(dto.Uid, HttpContext);

        if (!result.Success)
            return Unauthorized(new { message = result.ErrorMessage });

        var token = _tokenService.CreateToken(result.Value!);
        return Ok(new
        {
            token,
            user = new
            {
                result.Value!.Id,
                result.Value.Username,
                result.Value.Role,
                result.Value.LoginMethode
            }
        });
    }

    /// <summary>
    /// Login per QR-Token (z. B. mobile App).
    /// </summary>
    [HttpPost("qr")]
    public async Task<IActionResult> QrLogin([FromBody] QrLoginDTO dto)
    {
        var result = await _userService.LoginByQrAsync(dto.Token, HttpContext);

        var audit = new LoginAudit
        {
            LoginMethod = LoginMethod.QRCode,
            Erfolgreich = result.Success,
            IPAdresse = HttpContext.Connection.RemoteIpAddress?.ToString(),
            Ort = "QR-Terminal",
            UserId = result.Success ? result.Value!.Id : null
        };
        await _loginAuditRepository.AddAsync(audit);

        if (!result.Success)
            return Unauthorized(new { message = result.ErrorMessage });

        var token = _tokenService.CreateToken(result.Value!);
        return Ok(new
        {
            token,
            user = new
            {
                result.Value!.Id,
                result.Value.Username,
                result.Value.Role,
                result.Value.LoginMethode
            }
        });
    }

    /// <summary>
    /// Generiert einen temporären QR-Token für den aktuellen Benutzer.
    /// </summary>
    [Authorize]
    [HttpPost("qr/generate")]
    public async Task<IActionResult> GenerateQrToken()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "Benutzer nicht gefunden." });

        user.QrToken = Guid.NewGuid().ToString();
        user.QrTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);

        await _userRepository.UpdateAsync(user);

        return Ok(new
        {
            token = user.QrToken,
            gültigBis = user.QrTokenExpiresAt
        });
    }
}
