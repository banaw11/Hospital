using HospitalAPI.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.DTOs
{
    public class NewEmployeeDetailsDTO
    {
        [Required]
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public Profession Profession { get; set; }
        public Specialization Specialization { get; set; }
        public string RtPPNumber { get; set; }
    }
}
