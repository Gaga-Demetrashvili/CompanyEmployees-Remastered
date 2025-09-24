using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.IntegrationTests.Factories;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CompanyEmployees.IntegrationTests.Tests;

public class SortingPagingFilteringSearchingTests : IClassFixture<CompanyEmployeesTestcontainersFactory>
{
    private readonly HttpClient _client;
    private const string EmployeesUrl = "/api/companies";

    public SortingPagingFilteringSearchingTests(CompanyEmployeesTestcontainersFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task WhenGetEndpointsRequested_ThenReturnsOKAndEntities()
    {
        // Arrange
        _client.DefaultRequestHeaders.Add("Accept", "application/vnd.codemaze.apiroot+json");

        // Act
        var response = await _client.GetAsync($"{EmployeesUrl}/C9D4C053-49B6-410C-BC78-2D54A9991870/employees");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299

        var result = await response.Content.ReadFromJsonAsync<List<Entity>>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, result?.Count);
        Assert.True(response.Headers.Contains("X-Pagination"));
    }

    [Fact]
    public async Task WhenPaging_ThenReturnsOKAndEntity()
    {
        // Arrange
        _client.DefaultRequestHeaders.Add("Accept", "application/vnd.codemaze.apiroot+json");

        var url = $"{EmployeesUrl}/C9D4C053-49B6-410C-BC78-2D54A9991870/employees";
        var queryParams = new Dictionary<string, string>
        {
            ["pageNumber"] = "2",
            ["pageSize"] = "1"
        };
        var urlWithParams = QueryHelpers.AddQueryString(url, queryParams);

        // Act
        var response = await _client.GetAsync(urlWithParams);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299

        var result = await response.Content.ReadFromJsonAsync<List<Entity>>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, result?.Count);
        Assert.True(response.Headers.Contains("X-Pagination"));
    }
}