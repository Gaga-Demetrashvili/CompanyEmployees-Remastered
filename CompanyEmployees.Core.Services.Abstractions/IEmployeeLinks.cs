using CompanyEmployees.Core.Domain.LinkModels;
using CompanyEmployees.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace CompanyEmployees.Core.Services.Abstractions;

public interface IEmployeeLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto,
        string fields, Guid companyId, HttpContext httpContext);
}