using CompanyEmployees.IntegrationTests.Factories;
using CompanyEmployees.Shared.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CompanyEmployees.IntegrationTests.Tests;

public class CompaniesResponseTests : IClassFixture<CompanyEmployeesTestcontainersFactory>
{
    private readonly HttpClient _client;
    private const string CompaniesUrl = "/api/companies";

    public CompaniesResponseTests(CompanyEmployeesTestcontainersFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData($"{CompaniesUrl}/3d490a70-94ce-4d15-9494-5248280c2ce3")]
    [InlineData($"{CompaniesUrl}/c9d4c053-49b6-410c-bc78-2d54a9991870")]
    public async Task WhenGetEndpointsRequested_ThenReturnsOKStatusCode(string url)
    {
        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CompanyDto>();
        Assert.IsType<CompanyDto>(result);
        Assert.False(string.IsNullOrEmpty(result.Name));
        Assert.False(string.IsNullOrEmpty(result.FullAddress));
    }

    [Fact]
    public async Task WhenCreateNewEntityRequested_ThenReturns201Created()
    {
        // Arrange
        var company = new CompanyForCreationDto
        {
            Name = "Test",
            Address = "TestAddress",
            Country = "USA"
        };
        // Act
        var response = await _client.PostAsJsonAsync(CompaniesUrl, company);
        var location = response.Headers.Location;
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(location);
    }
}
