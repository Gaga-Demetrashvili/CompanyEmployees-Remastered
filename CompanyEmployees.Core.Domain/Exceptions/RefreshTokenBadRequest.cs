namespace CompanyEmployees.Core.Domain.Exceptions;

public class RefreshTokenBadRequest : BadRequestException
{
    public RefreshTokenBadRequest() : base("The refresh token is invalid or has expired.")
    {
    }
}
