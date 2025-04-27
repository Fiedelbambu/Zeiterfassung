using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Application.DTOs.User
{
    public class RegisterUserDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Benutzername muss mindestens 3 Zeichen haben.")]
        public string Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse.")]
        public string? Email { get; set; }  // <-- ACHTUNG: Optional (kein [Required])

        [Required]
        [MinLength(10, ErrorMessage = "Passwort muss mindestens 10 Zeichen haben.")]
        public string Passwort { get; set; } = string.Empty;
    }


}
