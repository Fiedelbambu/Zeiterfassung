using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Application.DTOs.User
{
    public class RegisterUserDTO
    {
        public string? Username { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Passwort { get; set; } = string.Empty;
    }

}
