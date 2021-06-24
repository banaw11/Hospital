using HospitalAPI.DTOs;
using HospitalAPI.DTOs.Pagination;
using HospitalAPI.Helpers.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalAPI.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedList<T>> GetPaginatedDetails<T>(EmployeesPaginationQuery paginationQuery, bool isAdministator);
        Task UpdateEmployeeDetails(NewEmployeeDetailsDTO dto);
        Task<EmployeeDetailsDTO> GetDetailsForUser(string login);
        Task<List<BasicEmployeeDetailsDTO>> GetUsers(Profession proffesion, Specialization? specialization);
    }
}
