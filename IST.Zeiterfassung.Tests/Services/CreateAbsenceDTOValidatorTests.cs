using FluentAssertions;
using FluentValidation.TestHelper;
using IST.Zeiterfassung.Application.DTOs.Absence;
using IST.Zeiterfassung.Application.Validators;
using IST.Zeiterfassung.Domain.Enums;
using System;
using Xunit;

namespace IST.Zeiterfassung.Tests.Validators
{
    public class CreateAbsenceDTOValidatorTests
    {
        private readonly CreateAbsenceDTOValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_StartDate_After_EndDate()
        {
            var dto = new CreateAbsenceDTO
            {
                StartDate = DateTime.Today.AddDays(3),
                EndDate = DateTime.Today,
                Typ = AbsenceType.Urlaub,
                Reason = "Test"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.StartDate)
                  .WithErrorMessage("Startdatum darf nicht nach dem Enddatum liegen.");
        }

        [Fact]
        public void Should_Have_Error_When_Reason_Too_Long()
        {
            var dto = new CreateAbsenceDTO
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Typ = AbsenceType.HomeOffice,
                Reason = new string('x', 600)
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Reason);
        }

        [Fact]
        public void Should_Have_Error_When_Typ_Invalid()
        {
            var dto = new CreateAbsenceDTO
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Typ = (AbsenceType)999, // ungültiger Enum-Wert
                Reason = "Test"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Typ);
        }

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var dto = new CreateAbsenceDTO
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                Typ = AbsenceType.Krankheit,
                Reason = "Bin krank"
            };

            var result = _validator.TestValidate(dto);

            result.IsValid.Should().BeTrue();
        }
    }
}
