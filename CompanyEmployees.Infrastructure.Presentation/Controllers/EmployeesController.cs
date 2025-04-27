using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Shared.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForCreationDto object is null");

        var employeeToReturn = _service.EmployeeService.CreateEmployeeForCompany(companyId, employee,
            trackChanges: false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id },
            employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);

        return NoContent();
    }

    // NOTE: We’ve changed only the Age property, but we have sent all the other properties with unchanged values as well.
    // Therefore, Age is only updated in the database.
    // But if we send the object with just the Age property, other properties will be set to their default values
    // and the whole object will be updated — not just the Age column.
    // That’s because the **** PUT is a request for a full update. **** This is very important to know.

    // One note, though. If we use the Update method from our repository, even if we change just the Age property,
    // all properties will be updated in the database.
    // In my current implementation only age was updated in query:

    // SET IMPLICIT_TRANSACTIONS OFF;
    // SET NOCOUNT ON;
    // UPDATE[Employees] SET[Age] = @p0
    // OUTPUT 1
    // WHERE[EmployeeId] = @p1;
    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id,
    [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForUpdateDto object is null");

        _service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true);

        return NoContent();
    }

    // If we want to update our resources only partially, we should use PATCH
    // For the PATCH request’s media type, we should use application/json-patch+json.
    // Even though application/json would be accepted in ASP.NET Core for the PATCH request,
    // The recommendation by REST standards is to use the second one.
    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
    [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null.");

        var result = _service.EmployeeService.GetEmployeeForPatch(companyId, id, compTrackChanges: false,
            empTrackChanges: true);

        patchDoc.ApplyTo(result.employeeToPatch);

        _service.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }
}