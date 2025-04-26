using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Infrastructure.Presentation.ModelBinders;
using CompanyEmployees.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

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

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    public CompaniesController(IServiceManager service)
    {
        _service = service;
    }

    // The purpose of the action methods inside the Web API controllers is not only to return results.
    // It is the main purpose, but not the only one. We need to pay attention to the status codes of our Web API responses as well.
    // Additionally, we will decorate our actions with the HTTP attributes which will specify the type of the HTTP request to that action.
    [HttpGet]
    // The IActionResult interface supports using a variety of methods, which return not only the result but also the status codes.
    // In this situation, the OK method returns all the companies and also the status code 200 — which stands for OK.
    // If an exception occurs, we are going to return the internal server error with the status code 500.

    // Because there is no route attribute right above the action,
    // the route for the GetAllCompanies action will be api/companies which is the route placed on top of our controller.
    public IActionResult GetAllCompanies()
    {
        var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);

        return Ok(companies);
    }

    // :guid is route constraint
    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, trackChanges: false);

        return Ok(company);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
    {
        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");

        var createdCompany = _service.CompanyService.CreateCompany(company);

        return CreatedAtRoute("CompanyById",
            new { id = createdCompany.Id },
            createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = _service.CompanyService.GetByIds(ids, trackChanges: false);

        return Ok(companies);
    }

    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result = _service.CompanyService.CreateCompanyCollection(companyCollection);

        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }
}