using FluentValidation;
using IST.Zeiterfassung.Application.DTOs.Absence;

namespace IST.Zeiterfassung.Application.Validators;

public class CreateAbsenceValidator : AbstractValidator<CreateAbsenceDTO>
{
    public CreateAbsenceValidator()
    {
        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Startdatum muss vor dem Enddatum liegen.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Grund für die Abwesenheit darf nicht leer sein.")
            .MaximumLength(200)
            .WithMessage("Grund darf maximal 200 Zeichen lang sein.");
    }
}
