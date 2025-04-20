using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.DTOs.Absence
{
    public class AbsenceResponseDTO
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Reason { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        public string? Username { get; set; }

        public TimeSpan Duration { get; set; }

        public AbsenceType Typ { get; set; }
        public AbsenceStatus Status { get; set; }
        public DateTime ErstelltAm { get; set; }
    }
}

