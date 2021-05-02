using HospitalAPI.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PersonalId { get; set; }
        [Required]
        public Profession Profession { get; set; }
        public Specialization Specialization { get; set; }
        public int RtPPNumber { get; set; }
    }
}
