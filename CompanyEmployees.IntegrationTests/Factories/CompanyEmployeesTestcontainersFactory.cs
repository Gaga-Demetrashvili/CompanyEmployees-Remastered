using CompanyEmployees.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;
using Xunit;

namespace CompanyEmployees.IntegrationTests.Factories;

public class CompanyEmployeesTestcontainersFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string Database = "master";
    private const string Username = "sa";
    private const string Password = "yourStrong(!)Password";
    private const ushort MsSqlPort = 1433;
    private readonly IContainer _mssqlContainer;

    public CompanyEmployeesTestcontainersFactory() => 
        _mssqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")   
        .WithPortBinding(MsSqlPort, true)
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("SQLCMDUSER", Username)
        .WithEnvironment("SQLCMDPASSWORD", Password)
        .WithEnvironment("MSSQL_SA_PASSWORD", Password)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MsSqlPort))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var host = _mssqlContainer.Hostname;
        var port = _mssqlContainer.GetMappedPublicPort(MsSqlPort);
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<RepositoryContext>));

            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer($"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True")
                    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var appContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
            try
            {
                appContext.Database.Migrate(); // Apply all migrations to keep schema in sync
            }
            catch (Exception ex)
            {
                // Log or handle migration exceptions if needed
                throw;
            }
        });
    }

    public async Task InitializeAsync()
    {
        await _mssqlContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _mssqlContainer.DisposeAsync();
    }
}
