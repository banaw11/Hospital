using AutoMapper;
using HospitalAPI.DTOs;
using HospitalAPI.Entities;
using HospitalAPI.Helpers.Enums;

namespace HospitalAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterUserDTO, Employee>();
            CreateMap<Employee, CreatedUserAccountDTO>()
                .ForMember(u => u.Name, opt => opt.MapFrom(e => $"{e.FirstName}, {e.LastName}"));
        
        }
    }
}
