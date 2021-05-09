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

            CreateMap<Employee, BasicEmployeeDetailsDTO>()
                .ForMember(d => d.Name, opt => opt.MapFrom(e => $"{e.FirstName}, {e.LastName}"))
                .ForMember(d => d.ProfessionName, opt => opt.MapFrom(e => e.Profession.ToString()))
                .ForMember(d => d.SpecializationName, opt =>
                    opt.MapFrom(e => e.Specialization != Specialization.NULL ? e.Specialization.ToString() : null));

            CreateMap<Employee, EmployeeDetailsDTO>()
                .ForMember(d => d.ProfessionName, opt => opt.MapFrom(e => e.Profession.ToString()))
                .ForMember(d => d.SpecializationName, opt =>
                    opt.MapFrom(e => e.Specialization != Specialization.NULL ? e.Specialization.ToString() : null));

        }
    }
}
