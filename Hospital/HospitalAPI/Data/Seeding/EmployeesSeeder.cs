using HospitalAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HospitalAPI.Data.Seeding
{
    public class EmployeesSeeder
    {
        private readonly DataContext _dbContext;
        private readonly IPasswordHasher<Employee> _passwordHasher;

        public EmployeesSeeder(DataContext dbContext, IPasswordHasher<Employee> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task Seed()
        {
            if (await _dbContext.Employees.AnyAsync())
                return;

            var employeesData = await File.ReadAllTextAsync("Data/Seeding/EmployeesSeed.json");
            var employees = JsonSerializer.Deserialize<List<Employee>>(employeesData);

            if (employees.Count == 0)
                return;

            foreach (var employee in employees)
            {
                employee.PasswordHash = _passwordHasher.HashPassword(employee, "Pa$$w0rd");
                if(employee.Profession != Helpers.Enums.Profession.ADMINISTRATOR)
                {
                    employee.Login = await GenerateLogin();
                }
                
            }

            await _dbContext.Employees.AddRangeAsync(employees);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<string> GenerateLogin()
        {
            Random random = new Random();
            string login;

            do
            {
                login = null;

                for (int i = 0; i < 2; i++)
                    login += Convert.ToChar(random.Next(65, 90));

                login += random.Next(1000, 9999);

            } while (string.IsNullOrEmpty(login) ||
                await _dbContext.Employees.AnyAsync(e => e.Login == login));

            return login;
        }
    }
}
