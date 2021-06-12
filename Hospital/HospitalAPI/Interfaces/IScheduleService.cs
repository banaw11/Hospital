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
        Task<IEnumerable<ScheduleDTO>> GetSchedulesForUserAsync(string login);
    }
}
