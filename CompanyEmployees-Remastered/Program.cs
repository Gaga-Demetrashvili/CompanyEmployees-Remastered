using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Core.Services.DataShaping;
using CompanyEmployees.Core.Services.Hateoas;
using CompanyEmployees.Infrastructure.Presentation.ActionFilters;
using CompanyEmployees.Infrastructure.Presentation.Validators;
using CompanyEmployees.Shared.DataTransferObjects;
using CompanyEmployees_Remastered;
using CompanyEmployees_Remastered.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services
    .AddValidatorsFromAssemblyContaining(typeof(CompanyForCreationDtoValidator));
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
builder.Services.AddScoped<IEmployeeLinks, EmployeeLinks>();
builder.Services.ConfigureVersioning();

// To enable our custom responses. This is to override the default behavior of the [ApiController] attribute.
// With this, we are suppressing a default model state validation
// that is implemented due to the existence of the [ApiController] attribute in all API controllers.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();

// Without this code, our API wouldn’t work, and wouldn’t know where to route incoming requests.
// But now, our app will find all of the controllers inside the Presentation project and configure them with the framework.
// They are going to be treated the same as if they were defined conventionally.
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
}).AddXmlDataContractSerializerFormatters()
.AddCustomCSVFormatter()
.AddApplicationPart(typeof(CompanyEmployees.Infrastructure.Presentation.AssemblyReference).Assembly);

builder.Services.AddCustomMediaTypes();

builder.Host.UseSerilog((hostContext, configuration) =>
    configuration.ReadFrom.Configuration(hostContext.Configuration));

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();

var app = builder.Build();

// Configure the HTTP request pipeline.

//var logger = app.Services.GetRequiredService<ILoggerManager>();
//app.ConfigureExceptionHandler(logger);
app.UseExceptionHandler(opt => { });

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

// Adds endpoints for controller actions without specifying any routes
app.MapControllers();

app.Run();
