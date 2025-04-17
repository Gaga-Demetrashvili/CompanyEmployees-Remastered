using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Shared.DataTransferObjects;

namespace CompanyEmployees_Remastered;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Company, CompanyDto>()
			.ForCtorParam("FullAddress",
				opt => opt.MapFrom(x => $"{x.Address} {x.Country}"));
	}
}
