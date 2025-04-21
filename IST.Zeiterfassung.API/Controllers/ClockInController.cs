using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClockInController : ControllerBase
{
    private readonly ITimeEntryRepository _timeEntryRepository;

    public ClockInController(ITimeEntryRepository timeEntryRepository)
    {
        _timeEntryRepository = timeEntryRepository;
    }

    /// <summary>
    /// Automatische Zeiterfassung – erkennt Clock-In, Pause, Clock-Out basierend auf der Anzahl der Buchungen heute.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ClockIn()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var jetzt = DateTime.UtcNow;
        var heute = jetzt.Date;

        // Einträge für heute holen
        var eintraegeHeute = await _timeEntryRepository.GetTodayEntriesAsync(userId);
        var phase = eintraegeHeute.Count;

        switch (phase)
        {
            case 0:
                //  1. Scan: Arbeitsbeginn
                var startEintrag = new TimeEntry
                {
                    Id = Guid.NewGuid(),
                    ErfasstVonUserId = userId,
                    ErfasstFürUserId = userId,
                    ZeitmodellUserId = userId,
                    Start = jetzt,
                    Ende = jetzt, // wird später überschrieben
                    Beschreibung = "Arbeitsbeginn",
                    Date = heute,
                    ErfasstAm = jetzt,
                    Pausenzeit = TimeSpan.Zero
                };
                await _timeEntryRepository.AddAsync(startEintrag);
                return Ok(new { message = "Arbeitszeit gestartet.", phase = 1 });

            case 1:
                //  2. Scan: Pause starten – wir merken uns den Zeitpunkt über .Ende
                var laufenderEintrag = eintraegeHeute.FirstOrDefault();
                if (laufenderEintrag != null)
                {
                    laufenderEintrag.Ende = jetzt; // speichert Pausenstart
                    await _timeEntryRepository.UpdateAsync(laufenderEintrag);
                }
                return Ok(new { message = "Pause gestartet.", phase = 2 });

            case 2:
                // 3. Scan: Pause beenden – Pausenzeit = jetzt - Pausenstart
                var aktiverEintrag = eintraegeHeute.FirstOrDefault();
                if (aktiverEintrag != null && aktiverEintrag.Ende > aktiverEintrag.Start)
                {
                    aktiverEintrag.Pausenzeit += jetzt - aktiverEintrag.Ende;
                    aktiverEintrag.Ende = jetzt; // Startzeit für restlichen Arbeitstag
                    await _timeEntryRepository.UpdateAsync(aktiverEintrag);
                }
                return Ok(new { message = "Pause beendet.", phase = 3 });

            case 3:
                //  4. Scan: Arbeitsende → endgültiges Ende setzen
                var ersterEintrag = eintraegeHeute.FirstOrDefault();
                if (ersterEintrag != null)
                {
                    ersterEintrag.Ende = jetzt;
                    await _timeEntryRepository.UpdateAsync(ersterEintrag);
                }
                return Ok(new { message = "Arbeitszeit beendet.", phase = 4 });

            default:
                return BadRequest(new { message = "Alle Phasen für heute bereits abgeschlossen." });
        }
    }

    /// <summary>
    /// Klassische Buchung ohne Phasenerkennung (manuelle Methode).
    /// </summary>
    [Authorize]
    [HttpPost("manual")]
    public async Task<IActionResult> ManualClockIn()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var entry = new TimeEntry
        {
            Id = Guid.NewGuid(),
            ErfasstVonUserId = userId,
            ErfasstFürUserId = userId,
            ZeitmodellUserId = userId,
            Start = DateTime.UtcNow,
            Ende = DateTime.UtcNow.AddHours(8), // statisch (manuell)
            Beschreibung = "Manuell erfasst",
            Date = DateTime.UtcNow.Date,
            ErfasstAm = DateTime.UtcNow
        };

        await _timeEntryRepository.AddAsync(entry);

        return Ok(new { message = "Manuelle Zeitbuchung erfolgreich erstellt", entry.Id });
    }
}
