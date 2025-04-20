using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class FeiertagController : ControllerBase
{
    private readonly IFeiertagRepository _repo;

    public FeiertagController(IFeiertagRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("{jahr:int}/{region}")]
    public async Task<IActionResult> Alle(int jahr, string region)
    {
        var result = await _repo.GetAlleAsync(jahr, region);
        return Ok(result.OrderBy(f => f.Datum));
    }

    [HttpPost]
    public async Task<IActionResult> Hinzufügen([FromBody] Feiertag dto)
    {
        await _repo.AddAsync(dto);
        return Ok(new { message = "Feiertag hinzugefügt." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Löschen(Guid id)
    {
        await _repo.RemoveAsync(id);
        return Ok(new { message = "Feiertag gelöscht." });
    }
}
