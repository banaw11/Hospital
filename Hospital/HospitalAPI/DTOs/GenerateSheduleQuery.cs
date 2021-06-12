using HospitalAPI.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.DTOs
{
    public class GenerateSheduleQuery
    {
        public Profession Profession { get; set; }
        public int Month { get; set; }
    }
}
