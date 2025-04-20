using CompanyEmployees.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

[Route("api/companies/{companyId}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;

	public EmployeesController(IServiceManager service) => _service = service;

    // we have the companyId parameter in our action and this parameter will be mapped from the main route.
    // For that, we didn’t place it in the [HttpGet] attribute as we did with the GetCompany action.
    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false);

        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);

        return Ok(employee);
    }
}