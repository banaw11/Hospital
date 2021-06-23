using HospitalAPI.DTOs;
using HospitalAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Interfaces
{
    public interface IScheduleService
    {
        Task GenerateScheduleForNursesAsync(int month);
        Task GenerateScheduleForDoctorsAsync(int month);
        Task DeleteSchedulesForMonth(int month);
        Task<IEnumerable<ScheduleDTO>> GetSchedulesForUserAsync(string login, int month);
        Task<IEnumerable<DateTime>> GetAvailableDaysForChange(int scheduleId);
        Task<IEnumerable<DateTime>> GetAvailableDaysForNew(string employeeLogin, int month);
        Task<bool> CreateNewSchedule(DateTime date, string employeeLogin);
        Task<bool> DeleteSchedule(int scheduleId);
        Task<bool> UpdateSchedule(int scheduleId, DateTime date);
    }
}
