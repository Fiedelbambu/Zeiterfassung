using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.DTOs.Absence
{
    public class CreateAbsenceDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AbsenceType Typ { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
