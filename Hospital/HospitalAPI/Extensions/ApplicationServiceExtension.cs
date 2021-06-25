using FluentValidation;
using HospitalAPI.Data;
using HospitalAPI.Data.Seeding;
using HospitalAPI.DTOs;
using HospitalAPI.DTOs.Pagination;
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
using System;

namespace HospitalAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                string connStr;

                // Depending on if in development or production, use either Heroku-provided
                // connection string, or development connection string from env var.
                if (env == "Development")
                {
                    // Use connection string from file.
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                else
                {
                    // Use connection string provided at runtime by Heroku.
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                    // Parse connection URL to connection string for Npgsql
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];

                    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
                }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from Heroku, use it to set up your DbContext.
                options.UseNpgsql(connStr);

                // connection string for SQLite DB
                //var connectionString = config.GetConnectionString("SqLiteConnection");
                //options.UseSqlite(connectionString);
            });
            services.AddScoped<EmployeesSeeder>();

            services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserDTOValidator>();
            services.AddScoped<IValidator<ResetPasswordDTO>, ResetPasswordDTOValidator>();
            services.AddScoped<IValidator<NewEmployeeDetailsDTO>, NewEmployeeDetailsDTOValidator>();
            services.AddScoped<IValidator<EmployeesPaginationQuery>, EmployeesPaginationQueryValidator>();
            services.AddScoped<IValidator<GenerateSheduleQuery>, GenerateScheduleQueryValidator>();

            return services;
        }
    }
}
