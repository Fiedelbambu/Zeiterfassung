using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IST.Zeiterfassung.Application.DTOs.Login;
using IST.Zeiterfassung.Application.Interfaces;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class LoginAuditController : ControllerBase
{
    private readonly ILoginAuditRepository _repo;

    public LoginAuditController(ILoginAuditRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Gibt alle Loginversuche im System zurück (Admin-only).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<LoginAuditDTO>>> GetAll()
    {
        var list = await _repo.GetAllAsync();

        var result = list.Select(e => new LoginAuditDTO
        {
            Zeitpunkt = e.Zeitpunkt,
            Username = e.User?.Username ?? "Unbekannt",
            IPAdresse = e.IPAdresse ?? "-",
            Erfolgreich = e.Erfolgreich,
            LoginMethod = e.LoginMethod,
            Ort = e.Ort
        }).ToList();

        return Ok(result);
    }
}
