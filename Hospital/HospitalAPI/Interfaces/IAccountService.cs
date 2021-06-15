using HospitalAPI.DTOs;
using System.Threading.Tasks;

namespace HospitalAPI.Interfaces
{
    public interface IAccountService
    {
        Task<CreatedUserAccountDTO> RegisterUser(RegisterUserDTO dto);
        Task<UserDTO> SignInUser(LoginUserDTO dto);
        Task ResetPassword(ResetPasswordDTO dto, bool isAdministrator, string userLogin);
    }
}
