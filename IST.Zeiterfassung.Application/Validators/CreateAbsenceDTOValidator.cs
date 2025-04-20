using FluentValidation;
using IST.Zeiterfassung.Application.DTOs.Absence;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.Validators
{
    public class CreateAbsenceDTOValidator : AbstractValidator<CreateAbsenceDTO>
    {
        public CreateAbsenceDTOValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Startdatum darf nicht leer sein.")
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("Startdatum darf nicht nach dem Enddatum liegen.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("Enddatum darf nicht leer sein.");

            RuleFor(x => x.Reason)
                .MaximumLength(500)
                .WithMessage("Kommentar darf maximal 500 Zeichen lang sein.");

            RuleFor(x => x.Typ)
                .IsInEnum()
                .WithMessage("Ungültiger Abwesenheitstyp.");
        }
    }
}
