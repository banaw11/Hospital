using AutoMapper;
using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.Entities;
using HospitalAPI.Helpers.Enums;
using HospitalAPI.Interfaces;
using HospitalAPI.Middlewares.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public ScheduleService(DataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task DeleteSchedulesForMonth(int month)
        {
            var schedules = await _dbContext.Schedules.Where(s => s.Date.Month == month).ToListAsync();

            _dbContext.Schedules.RemoveRange(schedules);

            await _dbContext.SaveChangesAsync();
        }

        public async Task GenerateScheduleForDoctorsAsync(int month)
        {
            Random random = new Random();
            List<Schedule> schedules = new List<Schedule>();
            int year = DateTime.Now.Year;
            List<DateTime> daysInMonth = new List<DateTime>(); 

            var specializations = ((Specialization[])Enum.GetValues(typeof(Specialization)))
                .ToList()
                .FindAll(e => e != Specialization.NULL);

            var doctors = await _dbContext.Employees
                .Where(e => e.Profession == Profession.DOCTOR)
                .ToListAsync();

            if(!doctors.Any())
            {
                throw new NotFoundException("There is no employees as Doctor in database");
            }

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                daysInMonth.Add(new DateTime(year, month, i));
            }

            foreach (var specialization in specializations)
            {
                var availableDoctors = doctors.Where(d => d.Specialization == specialization).ToList();

                foreach (var doctor in availableDoctors)
                {

                    var availableDates = daysInMonth
                        .Where(d => !schedules
                            .Any(s => (s.Date == d && (s.EmployeeLogin == doctor.Login || s.Employee.Specialization == doctor.Specialization)) ||
                                ((s.Date == d.AddDays(1) || s.Date == d.AddDays(-1)) && s.EmployeeLogin == doctor.Login )))
                        .ToList();

                    if(availableDates.Any())
                    {
                        int countDays = (daysInMonth.Count / availableDoctors.Count) > 10 ? 10 : daysInMonth.Count / availableDoctors.Count;
                        var days = availableDates
                            .OrderBy(d => random.Next())
                            .Take(countDays);

                        foreach (var day in days)
                        {
                            Schedule schedule = new Schedule()
                            {
                                Date = day,
                                Month = month,
                                EmployeeLogin = doctor.Login,
                                Employee = doctor
                            };
                            schedules.Add(schedule);
                        }
                    }
                }
            }

            _dbContext.Schedules.AddRange(schedules);
           await _dbContext.SaveChangesAsync();
        }

        public async Task GenerateScheduleForNursesAsync(int month)
        {
            Random random = new Random();
            List<Schedule> schedules = new List<Schedule>();
            List<DateTime> daysInMonth = new List<DateTime>();
            int year = DateTime.Now.Year;

            var nurses = await _dbContext.Employees
                .Where(e => e.Profession == Profession.NURSE)
                .ToListAsync();

            if(!nurses.Any())
            {
                throw new NotFoundException("There is no employees like Nurse in database");
            }

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                daysInMonth.Add(new DateTime(year, month, i));
            }

            foreach (var nurse in nurses)
            {
                for (int i = 0; i < random.Next(6, 10); i++)
                {
                    DateTime day;
                    do
                    {
                        day = daysInMonth[random.Next(0, daysInMonth.Count - 1)];
                    } while (schedules.Any(s => (s.Date == day && s.EmployeeLogin == nurse.Login) ||
                          (s.Date == day.AddDays(-1) && s.EmployeeLogin == nurse.Login) ||
                          (s.Date == day.AddDays(1) && s.EmployeeLogin == nurse.Login)));

                    Schedule schedule = new Schedule()
                    {
                        Date = day,
                        Month = month,
                        EmployeeLogin = nurse.Login
                    };
                    schedules.Add(schedule);
                }
            }

            _dbContext.Schedules.AddRange(schedules);
            await _dbContext.SaveChangesAsync();


        }

        public async Task<IEnumerable<ScheduleDTO>> GetSchedulesForUserAsync(string login)
        {
            var userExistAndHasShedules = _dbContext.Employees
                .Any(e => e.Login == login && e.Profession != Profession.ADMINISTRATOR);

            if(!userExistAndHasShedules)
            {
                throw new NotFoundException("Employee not found or is Administrator");
            }

            var schedules = await _dbContext.Schedules
                .Where(s => s.EmployeeLogin == login)
                .Select(s => _mapper.Map<ScheduleDTO>(s))
                .OrderBy(s => s.Date)
                .ToListAsync();

            return schedules;
        }
    }
}
