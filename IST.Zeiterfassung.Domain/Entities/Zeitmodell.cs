using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IST.Zeiterfassung.Domain.Entities
{
    public class Zeitmodell
    {
        public Guid Id { get; set; }
        public string Bezeichnung { get; set; } = "Standardmodell";

        public TimeSpan WochenSollzeit { get; set; }
        public bool IstGleitzeit { get; set; }

        // Tages-Sollzeiten: Mo–So
        [NotMapped]
        public Dictionary<DayOfWeek, TimeSpan> SollzeitProTag { get; set; } = new();

        // Optional: Gleitzeitkonto
        public bool GleitzeitkontoAktiv { get; set; } = false;
        public TimeSpan? GleitzeitMonatslimit { get; set; }
        public bool SaldoÜbertragAktiv { get; set; } = false;

        public double? GetSollzeitInStundenFor(DateOnly datum)
        {
            if (SollzeitProTag.TryGetValue(datum.DayOfWeek, out var timeSpan))
                return timeSpan.TotalHours;

            return null;
        }


        // Optional: Schichtsystem (später)
        // public string? SchichtTyp { get; set; }
    }

}
