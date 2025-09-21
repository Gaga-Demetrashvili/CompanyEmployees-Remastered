namespace CompanyEmployees.Core.Domain.ConfigurationModels;

public class JwtConfiguration
{
    public const string JwtSectionName = "JwtSettings";
    public string? ValidIssuer { get; set; }
    public string? ValidAudience { get; set; }
    public string? Expires { get; set; }
}
