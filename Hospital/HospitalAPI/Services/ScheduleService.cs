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

        public async Task<bool> CreateNewSchedule(DateTime date, string employeeLogin)
        {
            var user = await _dbContext.Employees.FindAsync(employeeLogin);

            if (user == null)
                throw new NotFoundException("User not found in database");

            Schedule schedule = new Schedule
            {
                Date = date,
                Month = date.Month,
                EmployeeLogin = employeeLogin,
                Employee = user
            };

            _dbContext.Schedules.Add(schedule);

            if (await _dbContext.SaveChangesAsync() == 1)
                return true;

            return false;
        }

        public async Task<bool> DeleteSchedule(int scheduleId)
        {
            var schedule = await _dbContext.Schedules.FindAsync(scheduleId);

            if (schedule == null)
                throw new NotFoundException("Schedule not found in database");

            _dbContext.Schedules.Remove(schedule);

            if (await _dbContext.SaveChangesAsync() == 1)
                return true;

            return false;
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
                            if(!schedules.Any(s => s.EmployeeLogin == doctor.Login && 
                                (s.Date == day.AddDays(1) || s.Date == day.AddDays(-1) || s.Date == day)))
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

        public async Task<IEnumerable<DateTime>> GetAvailableDaysForChange(int scheduleId)
        {
            var schedule = _dbContext.Schedules.Include(s => s.Employee).FirstOrDefault(s => s.Id == scheduleId);

            if(schedule == null)
            {
                throw new NotFoundException("Schedule not found");
            }

            int month = schedule.Month;
            Profession profession = schedule.Employee.Profession;
            Specialization specialization = schedule.Employee.Specialization;
            string employeeLogin = schedule.EmployeeLogin;

            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);

            List<DateTime> days = await GetAvailableDays(month, profession, employeeLogin, specialization);

            days.Add(schedule.Date);

            return days.OrderBy(d => d).ToList();

        }

        public async Task<IEnumerable<DateTime>> GetAvailableDaysForNew(string employeeLogin, int month)
        {
            var user = _dbContext.Employees.FirstOrDefault(e => e.Login == employeeLogin);

            if(user == null)
                throw new NotFoundException("User not found in database");

            List<DateTime> days = new List<DateTime>();

            var schedulesCount = _dbContext.Schedules
                .Where(s => s.EmployeeLogin == employeeLogin && s.Month == month && s.Date.Year == DateTime.Now.Year)
                .Count();

            if(schedulesCount >= 10)
                return days;

            days = await GetAvailableDays(month, user.Profession, employeeLogin, user.Specialization);

            return days;

        }

        public async Task<IEnumerable<ScheduleDTO>> GetSchedulesForUserAsync(string login, int month)
        {
            var userExist = _dbContext.Employees
                .Any(e => e.Login == login);

            if(!userExist)
            {
                throw new NotFoundException("Nie znaleziono pracownika");
            }

            var schedules = await _dbContext.Schedules
                .Where(s => s.EmployeeLogin == login && s.Month == month)
                .Select(s => _mapper.Map<ScheduleDTO>(s))
                .ToListAsync();
                

            return schedules.OrderBy(s => s.Date).ToList();
        }

        public async Task<bool> UpdateSchedule(int scheduleId, int day)
        {
            var schedule = await _dbContext.Schedules.FirstOrDefaultAsync(s => s.Id == scheduleId);

            if (schedule == null)
            {
                throw new NotFoundException("Schedule not found in databse");
            }
                

            if(day> DateTime.DaysInMonth(schedule.Date.Year,schedule.Date.Month) || day < 1)
            {
                throw new BadRequestException("Podany dzień jest nieprawidłowy");
            }
                

            var date = new DateTime(schedule.Date.Year, schedule.Date.Month, day);

            schedule.Date = date;
            _dbContext.Schedules.Update(schedule);

            if (await _dbContext.SaveChangesAsync() == 1)
                return true;

            return false;
        }

        private async Task<List<DateTime>> GetAvailableDays(int month, Profession profession, string employeeLogin, Specialization specialization)
        {
            List<DateTime> days = new List<DateTime>();
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year,month);

            for (int i = 1; i <= daysInMonth; i++)
            {
                days.Add(new DateTime(DateTime.Now.Year, month, i));
            }

            if (profession == Profession.NURSE)
            {
                var schedulesDays = _dbContext.Schedules
                    .Where(s => s.Month == month && s.EmployeeLogin == employeeLogin)
                    .Select(s => s.Date)
                    .ToList();

                List<DateTime> availableDays = new List<DateTime>();
                foreach (var day in days)
                {
                    if (!schedulesDays.Contains(day) && !schedulesDays.Contains(day.AddDays(-1)) && !schedulesDays.Contains(day.AddDays(1)))
                        availableDays.Add(day);
                }

                days = availableDays;
            }
            else if (profession == Profession.DOCTOR)
            {
                var otherSchedulesDays = await _dbContext.Schedules
                    .Where(s => s.Month == month && s.Date.Year == DateTime.Now.Year)
                    .Include(s => s.Employee)
                    .Where(s => s.EmployeeLogin != employeeLogin && s.Employee.Specialization == specialization)
                    .Select(s => s.Date)
                    .ToListAsync();

                var employeeSchedulesDays = _dbContext.Schedules
                    .Where(s => s.Month == month && s.Date.Year == DateTime.Now.Year && s.EmployeeLogin == employeeLogin)
                    .Select(s => s.Date)
                    .ToList();

                List<DateTime> availableDays = new List<DateTime>();
                foreach (var day in days)
                {
                    if (!employeeSchedulesDays.Contains(day) && !otherSchedulesDays.Contains(day) && !employeeSchedulesDays.Contains(day.AddDays(1)) && !employeeSchedulesDays.Contains(day.AddDays(-1)))
                        availableDays.Add(day);
                }

                days = availableDays;
            }

            return days;
        }

    }
}
