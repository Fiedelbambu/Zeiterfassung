using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.DTOs.User
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string? Username { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public Role Role { get; set; }

    }
}
