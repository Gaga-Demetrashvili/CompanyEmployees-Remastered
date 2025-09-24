using CompanyEmployees.IntegrationTests.Factories;
using CompanyEmployees.Shared.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CompanyEmployees.IntegrationTests.Tests;

public class ValidationTests : IClassFixture<CompanyEmployeesTestcontainersFactory>
{
    private readonly HttpClient _client;
    private const string CompaniesUrl = "/api/companies";

    public ValidationTests(CompanyEmployeesTestcontainersFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task WhenEntityDoesntExist_ThenReturns404NotFound()
    {
        // Act
        var response = await _client.GetAsync($"{CompaniesUrl}/d9d4c053-49b6-410c-bc78-2d54a9991870");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task WhenModelStateInvalidOnCreation_ThenReturns422UnprocessableEntity()
    {
        // Arrange 
        var company = new CompanyForCreationDto
        {
            Address = "TestAddress",
            Country = "USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync(CompaniesUrl, company);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }
}