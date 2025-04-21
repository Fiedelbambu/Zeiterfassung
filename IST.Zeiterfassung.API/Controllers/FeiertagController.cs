using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IST.Zeiterfassung.API.Controllers
{
    /// <summary>
    /// Stellt Funktionen zum Verwalten und Importieren von Feiertagen bereit.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class FeiertagController : ControllerBase
    {
        private readonly IFeiertagRepository _repo;
        private readonly FeiertagsImportService _importService;

        public FeiertagController(IFeiertagRepository repo, FeiertagsImportService importService)
        {
            _repo = repo;
            _importService = importService;
        }

        /// <summary>
        /// Fügt einen einzelnen Feiertag manuell hinzu.
        /// </summary>
        /// <param name="dto">Feiertagseintrag</param>
        /// <returns>Bestätigung</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Hinzufügen([FromBody] Feiertag dto)
        {
            await _repo.AddAsync(dto);
            return Ok(new { message = "Feiertag hinzugefügt." });
        }

        /// <summary>
        /// Importiert alle Feiertage für ein bestimmtes Jahr und Land (z. B. AT, DE, CH).
        /// </summary>
        /// <param name="jahr">Zieljahr (z. B. 2025)</param>
        /// <param name="land">Ländercode nach ISO-3166 (z. B. AT, DE, CH)</param>
        /// <returns>Liste importierter Feiertage</returns>
        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Import([FromQuery] int jahr = 2025, [FromQuery] string land = "AT")
        {
            try
            {
                var result = await _importService.LadeUndSpeichereFeiertageAsync(jahr, land);
                return Ok(new
                {
                    message = $"{result.Count} Feiertage importiert für {land} im Jahr {jahr}.",
                    daten = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Feiertage konnten nicht importiert werden.", details = ex.Message });
            }
        }
    }
}
