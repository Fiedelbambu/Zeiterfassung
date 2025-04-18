using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Domain.Entities
{
    public class Absence
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; } = string.Empty;
        public User User { get; set; } = new User();
        public TimeSpan Duration => EndDate - StartDate;

    }
}
