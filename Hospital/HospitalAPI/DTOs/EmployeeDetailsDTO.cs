using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.DTOs
{
    public class EmployeeDetailsDTO
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public string ProfessionName { get; set; }
        public string SpecializationName { get; set; }
        public string RtPPNumber { get; set; }
    }
}
