using CompanyEmployees.Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace CompanyEmployees.Core.Domain.LinkModels;

public record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);