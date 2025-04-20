using FluentValidation;
using IST.Zeiterfassung.Application.DTOs.TimeEntry;

namespace IST.Zeiterfassung.Application.Validators;

public class CreateTimeEntryValidator : AbstractValidator<CreateTimeEntryDTO>
{
    public CreateTimeEntryValidator()
    {
        RuleFor(x => x.Start)
            .LessThan(x => x.Ende)
            .WithMessage("Startzeit muss vor der Endzeit liegen.");

        RuleFor(x => x.Pausenzeit)
            .GreaterThanOrEqualTo(TimeSpan.Zero)
            .WithMessage("Pausenzeit darf nicht negativ sein.");
    }
}

