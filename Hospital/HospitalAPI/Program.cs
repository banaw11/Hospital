using HospitalAPI.Data;
using HospitalAPI.Data.Seeding;
using HospitalAPI.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try 
            {
                var dbContext = services.GetRequiredService<DataContext>();
                var passwordHasher = services.GetRequiredService<IPasswordHasher<Employee>>();

                await dbContext.Database.MigrateAsync();

                var seeder = services.GetRequiredService<EmployeesSeeder>();
                await seeder.Seed();
            }
            catch(Exception e)
            {

            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
