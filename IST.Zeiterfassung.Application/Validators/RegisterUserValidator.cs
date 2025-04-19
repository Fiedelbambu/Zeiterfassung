using FluentValidation;
using IST.Zeiterfassung.Application.DTOs.User;

namespace IST.Zeiterfassung.Application.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Benutzername ist erforderlich.")
            .MinimumLength(3).WithMessage("Mindestens 3 Zeichen erforderlich.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-Mail ist erforderlich.")
            .EmailAddress().WithMessage("Keine gültige E-Mail-Adresse.");

        RuleFor(x => x.Passwort)
            .NotEmpty().WithMessage("Passwort ist erforderlich.")
            .MinimumLength(6).WithMessage("Mindestens 6 Zeichen erforderlich.");
    }
}

