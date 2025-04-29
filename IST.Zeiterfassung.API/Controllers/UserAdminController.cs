using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IST.Zeiterfassung.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserAdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAdminController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gibt alle Benutzer für die Adminübersicht zurück.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Erstellt einen neuen Benutzer (nur für Admins).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            var result = await _userService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Value);
        }


        /// <summary>
        /// Weist einem Benutzer eine NFC-ID zu (z. B. für Terminal oder Chipkarte).
        /// </summary>
        [HttpPut("{id}/nfc")]
        public async Task<IActionResult> SetNfc(Guid id, [FromBody] SetNfcIdDTO dto)
        {
            var result = await _userService.SetNfcIdAsync(id, dto);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new { message = result.Value });
        }

        /// <summary>
        /// Setzt oder entfernt ein QR-Token für einen Benutzer.
        /// </summary>
        [HttpPut("{id}/qr")]
        public async Task<IActionResult> SetQrToken(Guid id, [FromBody] SetQrTokenDTO dto)
        {
            var result = await _userService.SetQrTokenAsync(id, dto);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new { message = result.Value });
        }

        [HttpGet("no-time-model")]
        public async Task<IActionResult> GetUsersWithoutTimeModel()
        {
            var users = await _userService.GetAllUsersAsync();

            var result = users
                .Where(u => u.ZeitmodellId == null)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role
                })
                .ToList();

            return Ok(result);
        }
        [HttpPut("{id}/zeitmodell")]
        public async Task<IActionResult> SetZeitmodell(Guid id, [FromBody] SetZeitmodellDTO dto)
        {
            var result = await _userService.SetZeitmodellAsync(id, dto);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new { message = result.Value });
        }
        [HttpGet("roles")]
        public IActionResult GetAllRoles()
        {
            var rollen = Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .Select(r => new { Wert = (int)r, Name = r.ToString() })
                .ToList();

            return Ok(rollen);
        }

        /// <summary>
        /// Aktualisiert die allgemeinen Daten eines Benutzers.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDTO dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new { message = "Benutzer erfolgreich aktualisiert." });
        }



    }
}