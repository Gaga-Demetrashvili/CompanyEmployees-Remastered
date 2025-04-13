using LoggingService;

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
}
