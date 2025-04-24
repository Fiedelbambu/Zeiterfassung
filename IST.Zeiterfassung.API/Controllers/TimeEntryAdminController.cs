using System.Text;
using IST.Zeiterfassung.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IST.Zeiterfassung.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TimeEntryAdminController : ControllerBase
    {
        private readonly ITimeEntryService _timeEntryService;

        public TimeEntryAdminController(ITimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService;
        }

        /// <summary>
        /// Gibt alle Zeitbuchungen im System zurück (Admin-only).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _timeEntryService.GetAllAsync();

            var dtoList = list.Select(t => new
            {
                t.Id,
                t.ErfasstFürUserId,
                Benutzer = t.Betroffener?.Username,
                Start = t.Start,
                Ende = t.Ende,
                Dauer = t.Dauer,
                Pause = t.Pausenzeit,
                Projekt = t.ProjektName,
                Beschreibung = t.Beschreibung,
                ErfasstAm = t.ErfasstAm
            });

            return Ok(dtoList);
        }
    
    [HttpGet("csv")]
        public async Task<IActionResult> ExportCsv([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
        {
            var entries = await _timeEntryService.GetAllAsync(from, to);

            var csv = new StringBuilder();
            csv.AppendLine("Benutzer,Start,Ende,Dauer,Pausenzeit,Projekt,Beschreibung");

            foreach (var e in entries)
            {
                var line = $"{e.Betroffener?.Username}," +
                           $"{e.Start:yyyy-MM-dd HH:mm}," +
                           $"{e.Ende:yyyy-MM-dd HH:mm}," +
                           $"{e.NettoDauer.TotalHours:F2}," +
                           $"{e.Pausenzeit.TotalMinutes}," +
                           $"\"{e.ProjektName}\"," +
                           $"\"{e.Beschreibung}\"";

                csv.AppendLine(line);
            }

            var fileName = $"Zeitbuchungen_{DateTime.UtcNow:yyyy_MM_dd}.csv";
            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", fileName);
        }

    }
}