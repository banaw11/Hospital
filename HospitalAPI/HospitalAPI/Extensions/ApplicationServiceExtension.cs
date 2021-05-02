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
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            var authenticationSettings = new AuthenticationSettings();
            config.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                };
            });

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

            return services;
        }
    }
}
