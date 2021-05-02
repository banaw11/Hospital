using FluentValidation;
using HospitalAPI.Data;
using HospitalAPI.Entities;
using HospitalAPI.Middlewares.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.DTOs.Validators
{
    public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
    {
        private readonly DataContext _dbContext;
        private readonly IPasswordHasher<Employee> _passwordHasher;

        public ResetPasswordDTOValidator(DataContext dbContext, IPasswordHasher<Employee> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;

            RuleFor(d => d.Login)
                .Must(value => _dbContext.Employees.Any(e => e.Login == value))
                .WithMessage("Podany login nie istnieje");

            RuleFor(d => d.Password)
                .MinimumLength(8).WithMessage("Hasło musi mieć conajmniej 8 znaków")
                .Matches("[A-Z]").WithMessage("Hasło musi zawierać wielką literę")
                .Matches("[a-z]").WithMessage("Hasło musi zawierać małą literę")
                .Matches("[0-9]").WithMessage("Hasło musi zawierać cyfrę 0-9");

            RuleFor(d => d.ConfirmPassword)
                .Equal(d => d.Password).WithMessage("Hasła nie są sobie równe");

            RuleFor(d => d.Password)
                .Must((dto, value) => CompareLastPasswordHash(dto.Login, value))
                .WithMessage("Nowe hasło nie może być takie samo jak poprzednie");

        }

        private bool CompareLastPasswordHash(string login, string newPassword)
        {
            var employee = _dbContext.Employees.FirstOrDefault(e => e.Login == login);

            if (employee is null)
                throw new NotFoundException("User not found");

            var haserResult = _passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, newPassword);

            if (haserResult == PasswordVerificationResult.Success)
                return false;

            return true;
        }
    }
}
