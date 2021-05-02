using FluentValidation;
using HospitalAPI.Data;
using HospitalAPI.Helpers.Enums;
using System.Collections.Generic;
using System.Linq;

namespace HospitalAPI.DTOs.Validators
{

    public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
    {
        private List<Specialization> specializations = new List<Specialization>()
        {
            Specialization.CARDIOLOGIST,
            Specialization.LARYNGOLOGIST,
            Specialization.NEUROLOGIST,
            Specialization.UROLOGIST
        };

        private List<Profession> professions = new List<Profession>()
        {
            Profession.ADMINISTRATOR,
            Profession.DOCTOR,
            Profession.NURSE
        };

        public RegisterUserDTOValidator(DataContext dbContext)
        {
          
            RuleFor(d => d.PersonalId)
                .Must(value => value.Length == 11).WithMessage("Numer PESEL nie jest prawidłowy");

            RuleFor(d => d.PersonalId)
                .Custom((value, context) =>
                {
                    var userExist = dbContext.Employees.Any(e => e.PersonalId == value);
                    if (userExist)
                        context.AddFailure("PersonalId", "Pracownik o danym numerze PESEL już istnieje w bazie danych");
                });

            RuleFor(d => d.Password)
                 .MinimumLength(8).WithMessage("Hasło musi mieć conajmniej 8 znaków")
                 .Matches("[A-Z]").WithMessage("Hasło musi zawierać wielką literę")
                 .Matches("[a-z]").WithMessage("Hasło musi zawierać małą literę")
                 .Matches("[0-9]").WithMessage("Hasło musi zawierać cyfrę 0-9");

            RuleFor(d => d.ConfirmPassword)
                .Equal(d => d.Password).WithMessage("Hasła nie są sobie równe");

            RuleFor(d => d.Profession)
                .IsInEnum()
                .WithMessage($"Zawód musi być jednym z [{string.Join(", ", professions)}]");

            RuleFor(d => d.Specialization)
                .Must((dto, value) =>
                     dto.Profession == Profession.DOCTOR ? specializations.Contains(value) : true)
                .WithMessage($"Lekarz musi mieć podaną jedną ze specjalizacji [{string.Join(", ", specializations)}]");

            RuleFor(d => d.RtPPNumber)
                .Must((dto, value) =>
                    dto.Profession == Profession.DOCTOR ? value.ToString().Length == 7 : true)
                .WithMessage($"Lekarz musi mieć podany poprawny numer prawa do wykonywania zawodu");

            RuleFor(d => d.RtPPNumber)
                .Custom((value, context) =>
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        var numberExist = dbContext.Employees.Any(e => e.RtPPNumber == value);
                        if (numberExist)
                            context.AddFailure("RtPPNumber", "Pracownik o danym numerze prawa do wykonywania zawodu już istnieje w bazie danych");
                    }
                });


        }

        
    }
}
