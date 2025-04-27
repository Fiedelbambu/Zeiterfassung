using System.ComponentModel.DataAnnotations.Schema;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Domain.Entities;

public class Absence
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; } // ➔ FK zuerst deklarieren!

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!; // ➔ Navigation Property (Required)

    public AbsenceType Typ { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AbsenceStatus Status { get; set; }
    public string? Kommentar { get; set; }
    public DateTime ErstelltAm { get; set; }

    public TimeSpan Dauer => EndDate - StartDate;

    public Absence() { }

    public Absence(Guid userId, AbsenceType typ, DateTime startDate, DateTime endDate, string? kommentar = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Typ = typ;
        StartDate = startDate;
        EndDate = endDate;
        Kommentar = kommentar;
        ErstelltAm = DateTime.UtcNow;
        Status = AbsenceStatus.Beantragt;
    }

    public string? Reason
    {
        get => Kommentar;
        set => Kommentar = value;
    }
}
