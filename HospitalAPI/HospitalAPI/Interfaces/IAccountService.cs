using HospitalAPI.DTOs;
using System.Threading.Tasks;

namespace HospitalAPI.Interfaces
{
    public interface IAccountService
    {
        Task<CreatedUserAccountDTO> RegisterUser(RegisterUserDTO dto);
        Task<string> SignInUser(LoginUserDTO dto);
    }
}
