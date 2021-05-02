using FluentValidation;
using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.DTOs.Validators;
using HospitalAPI.Entities;
using HospitalAPI.Helpers;
using HospitalAPI.Interfaces;
using HospitalAPI.Middlewares;
using HospitalAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                var connectionString = config.GetConnectionString("SqLiteConnection");
                options.UseSqlite(connectionString);
            });

            services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserDTOValidator>();
            services.AddScoped<IValidator<ResetPasswordDTO>, ResetPasswordDTOValidator>();

            return services;
        }
    }
}
