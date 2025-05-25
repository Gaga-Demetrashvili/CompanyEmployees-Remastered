using Asp.Versioning;
using CompanyEmployees.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

//[ApiVersion("2.0", Deprecated = true)]
//[Route("api/{v:apiversion}/companies")]
[Route("api/companies")]
[ApiController]
public class CompaniesV2Controller : ControllerBase
{
    private readonly IServiceManager _service;
    public CompaniesV2Controller(IServiceManager service) => _service = service;
    [HttpGet]

    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _service.CompanyService
            .GetAllCompaniesAsync(trackChanges: false);

        var companiesV2 = companies.Select(c => $"{c.Name} V2");

        return Ok(companiesV2);
    }
}