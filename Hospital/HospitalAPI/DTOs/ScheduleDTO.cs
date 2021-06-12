using System;

namespace HospitalAPI.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeLogin { get; set; }
    }
}
