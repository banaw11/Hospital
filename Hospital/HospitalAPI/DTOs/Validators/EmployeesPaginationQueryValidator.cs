using FluentValidation;
using HospitalAPI.DTOs.Pagination;
using HospitalAPI.Entities;
using HospitalAPI.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalAPI.DTOs.Validators
{
    public class EmployeesPaginationQueryValidator : AbstractValidator<EmployeesPaginationQuery>
    {
        List<string> columns = Employee.GetNames();
        List<string> professions = Enum.GetNames<Profession>().ToList();
        List<string> specializations = Enum.GetNames<Specialization>().ToList();
       
        public EmployeesPaginationQueryValidator()
        {
            RuleFor(q => q.FilterColumn)
                .Must(value => string.IsNullOrEmpty(value) || columns.Contains(value))
                .WithMessage($"Nazwa kolumny filtrowania musi zawierać się w [{string.Join("; ", columns)}] \n lub pozostać pusta");
            
            RuleFor(q => q.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || columns.Contains(value.ToUpper()))
                .WithMessage($"Nazwa kolumny sortowania musi zawierać się w [{string.Join("; ", columns)}] \n lub pozostać pusta");

            RuleFor(q => q.SearchPhrase)
                .Must((query, value) => query.FilterColumn == nameof(Employee.Profession).ToUpper() ?
                    string.IsNullOrEmpty(value) || professions.Contains(value) : true)
                .WithMessage($"Fraza filtrowania dla kolumny zawód musi zawierać się w [{string.Join("; ", professions)}] \n lub pozostać pusta");


            RuleFor(q => q.SearchPhrase)
                .Must((query, value) => query.FilterColumn == nameof(Employee.Specialization).ToUpper() ?
                    string.IsNullOrEmpty(value) || specializations.Contains(value) : true)
                .WithMessage($"Fraza filtrowania dla kolumny specjalizacja musi zawierać się w [{string.Join("; ", specializations)}] \n lub pozostać pusta");

        }
    }
}
