using IST.Zeiterfassung.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Holt den Monatsbericht für den aktuell eingeloggten Benutzer.
    /// </summary>
    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlyReport([FromQuery] int jahr, [FromQuery] int monat)
    {
        if (monat < 1 || monat > 12)
            return BadRequest(new { message = "Ungültiger Monat (1–12 erwartet)." });

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _reportService.GetMonthlyReportAsync(userId, jahr, monat);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(result.Value);
    }


    [HttpGet("monthly/csv")]
   
    public async Task<IActionResult> ExportMonthlyAsCsv([FromQuery] int jahr, [FromQuery] int monat)
    {
        if (monat < 1 || monat > 12)
            return BadRequest(new { message = "Ungültiger Monat (1–12 erwartet)." });

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _reportService.GetMonthlyReportAsync(userId, jahr, monat);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        var csv = new StringBuilder();
        csv.AppendLine("Datum,Gearbeitet,Pausenzeit,Nettozeit,Montage,Projekte");

        foreach (var tag in result.Value!.Tage)
        {
            var zeile = $"{tag.Datum:yyyy-MM-dd}," +
                        $"{tag.Gearbeitet.TotalHours:F2}h," +
                        $"{tag.Pause.TotalMinutes}min," +
                        $"{tag.Nettozeit.TotalHours:F2}h," +
                        $"{(tag.IstMontage ? "Ja" : "Nein")}," +
                        $"\"{string.Join(" / ", tag.Projekte)}\"";

            csv.AppendLine(zeile);
        }

        var filename = $"Monatsbericht_{jahr}_{monat:D2}.csv";
        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", filename);
    }
}