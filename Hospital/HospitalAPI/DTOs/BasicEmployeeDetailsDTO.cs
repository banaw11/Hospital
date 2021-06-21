using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.DTOs
{
    public class BasicEmployeeDetailsDTO
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string ProfessionName { get; set; }
        public string SpecializationName { get; set; }
    }
}
