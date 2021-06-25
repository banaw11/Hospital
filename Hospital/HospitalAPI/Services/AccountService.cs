using AutoMapper;
using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.Entities;
using HospitalAPI.Helpers;
using HospitalAPI.Interfaces;
using HospitalAPI.Middlewares.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _dbContext;
        private readonly IPasswordHasher<Employee> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(DataContext dbContext, IPasswordHasher<Employee> passwordHasher, IMapper mapper, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _authenticationSettings = authenticationSettings;
        }

        public async Task DeleteAccount(string login)
        {
            var account = await _dbContext.Employees.FindAsync(login);

            if(account == null)
            {
                throw new NotFoundException("Konto nie zostało znalezione");
            }

            _dbContext.Employees.Remove(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CreatedUserAccountDTO> RegisterUser(RegisterUserDTO dto)
        {
            var employe = _mapper.Map<Employee>(dto);

            if (_dbContext.Employees.Any(e => e.PersonalId == employe.PersonalId))
                throw new BadRequestException($"Użytkownik z podanym numerem PESEL {dto.PersonalId} istnieje już w bazie danych");

            employe.Login = GenerateLogin();
            employe.PasswordHash = _passwordHasher.HashPassword(employe, dto.Password);

            _dbContext.Add(employe);
           await _dbContext.SaveChangesAsync();

            var newUserAccount = _mapper.Map<CreatedUserAccountDTO>(employe);
            return newUserAccount;
        }

        public async Task ResetPassword(ResetPasswordDTO dto, bool isAdministrator, string userLogin)
        {
            if (!isAdministrator)
                if (userLogin != dto.Login)
                    throw new ForbidException("Nie masz uprawnień, aby zmienić hasło innemu użytkownikowi");

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Login == dto.Login);

            var newPasswordHash = _passwordHasher.HashPassword(employee, dto.Password);

            employee.PasswordHash = newPasswordHash;

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserDTO> SignInUser(LoginUserDTO dto)
        {
            var employee = await _dbContext.Employees
                 .FirstOrDefaultAsync(e => e.Login.ToUpper() == dto.Login.ToUpper());

            if (employee is null)
                throw new BadRequestException($"Nieprawidłowy login lub hasło");

            var hashResult = _passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, dto.Password);
            if (hashResult == PasswordVerificationResult.Failed)
                throw new BadRequestException($"Nieprawidłowy login lub hasło");

            var userDto = _mapper.Map<UserDTO>(employee);
            userDto.Token = GenerateToken(employee);

            return userDto;

        }

        public async Task UpdateAccount(EmployeeDetailsDTO dto)
        {
            var employee = await _dbContext.Employees.FindAsync(dto.Login);
            if(employee == null)
            {
                throw new NotFoundException("Pracownik nie został znaleziony");
            }

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.PersonalId = dto.PersonalId;

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        private string GenerateLogin ()
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
                _dbContext.Employees.Any(e => e.Login == login));

            return login;
        }

        private string GenerateToken(Employee employee)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Login),
                new Claim(ClaimTypes.Name, $"{employee.FirstName}, {employee.LastName}"),
                new Claim(ClaimTypes.Role, $"{employee.Profession}")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
