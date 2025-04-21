using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Domain.Entities
{
    public class LoginAudit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public LoginMethod LoginMethod { get; set; }
        public bool Erfolgreich { get; set; }
        public string? IPAdresse { get; set; }
        public string? Ort { get; set; }
        public DateTime Zeitpunkt { get; set; } = DateTime.UtcNow;
    }

}
