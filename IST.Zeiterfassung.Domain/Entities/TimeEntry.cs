using System.ComponentModel.DataAnnotations.Schema;

namespace IST.Zeiterfassung.Domain.Entities
{
    public class TimeEntry
    {
        public Guid Id { get; set; }

        // Wer hat die Zeit erfasst
        public Guid ErfasstVonUserId { get; set; }

        [ForeignKey(nameof(ErfasstVonUserId))]
        public User Erfasser { get; set; } = null!;

        // Für wen gilt die Zeitbuchung
        public Guid ErfasstFürUserId { get; set; }

        [ForeignKey(nameof(ErfasstFürUserId))]
        public User Betroffener { get; set; } = null!;

        // Kontext-Benutzer z. B. für Reports
        public Guid ZeitmodellUserId { get; set; }

        [ForeignKey(nameof(ZeitmodellUserId))]
        public User ZeitmodellUser { get; set; } = null!;

        public DateTime Start { get; set; }
        public DateTime Ende { get; set; }

        public string? Beschreibung { get; set; }
        public string? ProjektName { get; set; }

        public bool IstMontage { get; set; }
        public DateTime ErfasstAm { get; set; } = DateTime.UtcNow;

        public TimeSpan Pausenzeit { get; set; } = TimeSpan.Zero;

        public TimeSpan NettoDauer => Dauer - Pausenzeit;
        public TimeSpan Dauer => Ende - Start;
    }
}
