using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs
{
    public class LoginUserDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
