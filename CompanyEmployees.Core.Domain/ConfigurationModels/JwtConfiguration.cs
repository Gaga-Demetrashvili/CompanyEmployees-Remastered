namespace CompanyEmployees.Core.Domain.ConfigurationModels;

public class JwtConfiguration
{
    public static string Section { get; set; } = "JwtSettings";
    public string? ValidIssuer { get; set; }
    public string? ValidAudience { get; set; }
    public string? Expires { get; set; }
}
