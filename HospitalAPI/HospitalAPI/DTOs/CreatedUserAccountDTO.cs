using HospitalAPI.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.DTOs
{
    public class CreatedUserAccountDTO
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public Profession Profession { get; set; }
        public Specialization Specialization { get; set; }
    }
}
