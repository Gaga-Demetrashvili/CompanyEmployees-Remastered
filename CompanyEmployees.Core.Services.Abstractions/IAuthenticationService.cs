using CompanyEmployees.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace CompanyEmployees.Core.Services.Abstractions;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
}
