using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Authentifiziert einen Benutzer.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
    {
        var result = await _userService.LoginAsync(dto);
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        var token = _tokenService.CreateToken(result.Value!);
        var dtoResult = new UserResponseDTO
        {
            Id = result.Value!.Id,
            Username = result.Value.Username,
            Email = result.Value.Email,
            Role = result.Value.Role
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
}
