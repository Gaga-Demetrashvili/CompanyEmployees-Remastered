using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Infrastructure.Presentation.ModelBinders;
using CompanyEmployees.Shared.DataTransferObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace CompanyEmployees.Infrastructure.Presentation.Controllers;

// TODO: Because ASP.NET Core uses Dependency Injection everywhere, we need to have a reference to all of the projects in the solution from the main project.
// This allows us to configure our services inside the Program class.
// While this is exactly what we want to do, it introduces a big design flaw.
// What’s preventing our controllers from injecting anything they want inside the constructor?
// By creating controllers in separate project we impose some strict rules about what controllers can do.

// <ItemGroup>
//    <FrameworkReference Include = "Microsoft.AspNetCore.App" />
// </ ItemGroup >
// This has to be added to the presentation project for it to have access to ContollerBase class.

// Presentation project only references Services.Abstractions for service contracts, because Services.Abstraction itself references only Domain project.
// That way we impose some strict rules about what controllers can do as well.

// Well, the [ApiController] attribute is applied to a controller class to enable the following opinionated, API-specific behaviors:

// Attribute routing requirement
// Automatic HTTP 400 responses
// Binding source parameter inference
// Multipart/form-data request inference
// Problem details for error status codes

// As you can see, it handles the HTTP 400 responses, and in our case, since the request’s body is null,
// the [ApiController] attribute handles that and returns the 400 (BadReqeust) response before the request even hits the CreateCompany action.
// This is useful behavior, but it prevents us from sending our custom responses to the client with
// different messages and status codes. This will be very important once we get to the Validation.

// This also means we can solve the same problem differently, by commenting out or removing the [ApiController] attribute only,
// without additional code for suppressing validation in Program class. It’s all up to you.
// But we like keeping it in our controllers because, as you could’ve seen,
// it provides additional functionalities other than just 400 – Bad Request responses.

// By default, we don’t have to use the ModelState.IsValid expression in Web API projects because,
// as we explained in one of the previous sections, controllers are decorated with the [ApiController] attribute.
// But, as we saw, it defaults all the model state errors to 400 – BadRequest and doesn’t allow us to
// return our custom error messages with a different status code. So, we suppressed it in the Program class.

// The response status code, when validation fails, should be 422 Unprocessable Entity.
// It means the server understood the content type of the request and the syntax of the request entity is correct,
// but it was unable to process validation rules applied on the entity inside the request body.
// If we don’t suppress the model validation from the [ApiController] attribute,
// we won’t be able to return this status code (422) since, as we said, it would default to 400.

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;
    //private readonly IValidator<CompanyForCreationDto> _postValidator;
    //private readonly IValidator<CompanyForUpdateDto> _putValidator;

    public CompaniesController(
        IServiceManager service
        //IValidator<CompanyForCreationDto> postValidator, 
        //IValidator<CompanyForUpdateDto> putValidator
        )
    {
        _service = service;
        //_postValidator = postValidator;
        //_putValidator = putValidator;
    }

    // The purpose of the action methods inside the Web API controllers is not only to return results.
    // It is the main purpose, but not the only one. We need to pay attention to the status codes of our Web API responses as well.
    // Additionally, we will decorate our actions with the HTTP attributes which will specify the type of the HTTP request to that action.
    [HttpGet(Name = "GetCompanies")]
    // The IActionResult interface supports using a variety of methods, which return not only the result but also the status codes.
    // In this situation, the OK method returns all the companies and also the status code 200 — which stands for OK.
    // If an exception occurs, we are going to return the internal server error with the status code 500.

    // Because there is no route attribute right above the action,
    // the route for the GetAllCompanies action will be api/companies which is the route placed on top of our controller.
    public async Task<IActionResult> GetAllCompanies(CancellationToken ct)
    {
        var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false, ct);

        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id, CancellationToken ct)
    {
        var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false, ct);

        return Ok(company);
    }

    [HttpPost(Name = "CreateCompany")]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company,
        [FromServices] IValidator<CompanyForCreationDto> validator, CancellationToken ct)
    {
        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");

        var valResult = validator.Validate(company);
        if (!valResult.IsValid)
            return UnprocessableEntity(valResult.ToDictionary());

        var createdCompany = await _service.CompanyService.CreateCompanyAsync(company, ct);

        return CreatedAtRoute("CompanyById",
            new { id = createdCompany.Id },
            createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids, CancellationToken ct)
    {
        var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false, ct);

        return Ok(companies);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection, CancellationToken ct)
    {
        var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection, ct);

        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id, CancellationToken ct)
    {
        await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false, ct);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company,
        [FromServices] IValidator<CompanyForUpdateDto> validator, CancellationToken ct)
    {
        if (company is null)
            return BadRequest("CompanyForUpdateDto object is null");

        var valResult = validator.Validate(company);
        if (!valResult.IsValid)
            return UnprocessableEntity(valResult.ToDictionary());

        await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true, ct);
        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetCompaniesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

        return Ok();
    }
}