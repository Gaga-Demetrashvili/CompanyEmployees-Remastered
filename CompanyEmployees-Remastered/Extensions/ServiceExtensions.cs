using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Core.Services;
using CompanyEmployees.Infrastructure.Persistence;
using LoggingService;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees_Remastered.Extensions;

public static class ServiceExtensions
{
    //  Why Postman Works but the Browser Fails

    //  Postman does not enforce CORS — it sends the request directly.
    //  Browsers do enforce CORS as a security feature, so they block frontend JavaScript apps from calling APIs that don’t explicitly allow them.

    //  If your frontend is sending cookies or using credentials: 'include' in fetch or Axios, then:
    //  The browser will require Access - Control - Allow - Credentials: true
    //  And you can’t use AllowAnyOrigin() with AllowCredentials()
    //  Instead you should use specific origin because Browser blocks frontend JavaScript apps from calling APIs that don’t explicitly allow them
    //  So if frontend app is calling this api, you should have following cors config:

    //          builder.WithOrigins("http://localhost:3000") // Replace with your frontend URL
    //              .AllowAnyMethod()
    //              .AllowAnyHeader()
    //              .AllowCredentials();
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISServerOptions>(options =>
        {

        });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    // There is a shortcut method AddSqlServer for this. However, it doesn’t provide all of the features the AddDbContext method provides.
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
    services.AddDbContext<RepositoryContext>(opts =>
        opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

    // This method replaces both AddDbContext and UseSqlServer methods and allows for more straightforward configuration.
    // However, it doesn’t provide all of the features the AddDbContext method provides.
    // So, for more advanced options, it is recommended to use AddDbContext. We will use it throughout the rest of the project.
    //public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
    //    services.AddSqlServer<RepositoryContext>((configuration.GetConnectionString("sqlConnection")));

    public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
}