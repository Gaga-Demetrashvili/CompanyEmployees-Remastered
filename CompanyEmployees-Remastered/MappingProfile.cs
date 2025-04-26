using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Shared.DataTransferObjects;

namespace CompanyEmployees_Remastered;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        //CreateMap<Company, CompanyDto>()
        //	.ForCtorParam("FullAddress",
        //		opt => opt.MapFrom(x => $"{x.Address} {x.Country}"));

        // When we use nomianl record, so we have properties in our object we should use ForMember method.
        // For positional records which we initialize through constructor we use ForCtorParam.

        CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => $"{x.Address} {x.Country}"));

        CreateMap<Employee, EmployeeDto>();

        CreateMap<CompanyForCreationDto, Company>();

        CreateMap<EmployeeForCreationDto, Employee>();
    }
}
