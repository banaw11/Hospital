using AutoMapper;
using AutoMapper.QueryableExtensions;
using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.DTOs.Pagination;
using HospitalAPI.Entities;
using HospitalAPI.Helpers.Enums;
using HospitalAPI.Interfaces;
using HospitalAPI.Middlewares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace HospitalAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeService(DataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedList<T>> GetPaginatedDetails<T>(EmployeesPaginationQuery paginationQuery, bool isAdministrator)
        {
            IQueryable<Employee> query;

            if(!isAdministrator)
                query = _dbContext.Employees.Where(e => e.Profession == Profession.DOCTOR || e.Profession == Profession.NURSE);
            else
                query = _dbContext.Employees;

            if (query.Count() < 1)
                throw new NotFoundException("Employees not found");


            if (!string.IsNullOrEmpty(paginationQuery.SearchPhrase) && !string.IsNullOrEmpty(paginationQuery.FilterColumn))
                query = query.Where(CreateCompareExpression<Employee>(paginationQuery.FilterColumn, paginationQuery.SearchPhrase));

            if(!string.IsNullOrEmpty(paginationQuery.SortBy))
            {
                var columnSelector = Employee.GetDictionaryOfProperties();
                var columnToOrder = columnSelector[paginationQuery.SortBy];
                query = query.OrderBy(columnToOrder);
            }

            var queryDTO = _mapper.ProjectTo<T>(query);

            if(queryDTO.Count() < 1)
                throw new NotFoundException("Results of pagination query not found");

            var pagedList = await PagedList<T>.CreateAsync(queryDTO, paginationQuery.PageNumber, paginationQuery.PageSize);


            return pagedList;
        }

        public async Task UpdateEmployeeDetails(NewEmployeeDetailsDTO dto)
        {
            var employee = await _dbContext.Employees.FindAsync(dto.Login);

            employee.FirstName = dto.FirstName != employee.FirstName && dto.FirstName != null ? dto.FirstName : employee.FirstName;
            employee.LastName = dto.LastName != employee.LastName && dto.LastName != null ? dto.LastName : employee.LastName;
            employee.PersonalId = dto.PersonalId != employee.PersonalId && dto.PersonalId != null ? dto.PersonalId : employee.PersonalId;
            employee.Profession = dto.Profession;
            employee.Specialization = dto.Specialization;
            employee.RtPPNumber = dto.RtPPNumber != employee.RtPPNumber && dto.RtPPNumber != null ? dto.RtPPNumber : employee.RtPPNumber;

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        private  Expression<Func<T,bool>> CreateCompareExpression<T>(string propertyName, string compareValue)
        {
            var t = Expression.Parameter(typeof(T), "e");
            var property = Expression.PropertyOrField(t, propertyName);

            MethodCallExpression propertyToUpper = null ;
            MethodInfo method;
            ConstantExpression value;

            if (property.Type.BaseType == typeof(Enum))
            {

                var searchedEnumValue = Enum.Parse(property.Type, compareValue);
                value = Expression.Constant(searchedEnumValue, typeof(object));
                method = property.Type.GetMethod("Equals", new[] { searchedEnumValue.GetType() });

            }
            else
            {
                propertyToUpper = Expression.Call(property, "ToUpper", null);
                method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                value = Expression.Constant(compareValue, typeof(string));
            }

            var body = Expression.Call(propertyToUpper != null ? propertyToUpper : property, method, value);
            var lambda = Expression.Lambda<Func<T, bool>>(body, t);

            return lambda;

        }

    }
}
