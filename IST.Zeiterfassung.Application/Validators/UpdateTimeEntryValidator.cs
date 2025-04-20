using FluentValidation;
using IST.Zeiterfassung.Application.DTOs.TimeEntry;

namespace IST.Zeiterfassung.Application.Validators;

public class UpdateTimeEntryValidator : AbstractValidator<UpdateTimeEntryDTO>
{
    public UpdateTimeEntryValidator()
    {
        RuleFor(x => x.Start)
            .LessThan(x => x.Ende)
            .WithMessage("Startzeit muss vor Endzeit liegen.");

        RuleFor(x => x.Pausenzeit)
            .GreaterThanOrEqualTo(TimeSpan.Zero);
    }
}
