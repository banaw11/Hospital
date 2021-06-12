using FluentValidation;
using HospitalAPI.Data;
using HospitalAPI.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.DTOs.Validators
{
    public class GenerateScheduleQueryValidator : AbstractValidator<GenerateSheduleQuery>
    {
        public GenerateScheduleQueryValidator(DataContext dbContext)
        {
            RuleFor(q => q.Profession)
                .Must(value => value != 0 && (value == Profession.DOCTOR || value == Profession.NURSE))
                .WithMessage($"Zawód jest wymagany i musi zawierać się w : [{Profession.DOCTOR}, {Profession.NURSE}]");

            RuleFor(q => q.Month)
                .Must(value => value > 0 && value <= 12)
                .WithMessage($"Miesiąc musi być wartością całkowitą z przedziału 1 - 12");


            RuleFor(q => q)
                .Custom((value, context) =>
                {
                    var exist = dbContext.Schedules
                        .Include(s => s.Employee)
                        .Any(s => s.Month == value.Month && s.Employee.Profession == value.Profession);
                    if (exist)
                    {
                        context.AddFailure($"Grafik dla zawodu {value.Profession} w miesiącu {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value.Month)} istnieje");
                    }
                });
        }
    }
}
