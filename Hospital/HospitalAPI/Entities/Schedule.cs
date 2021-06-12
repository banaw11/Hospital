using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeLogin { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
