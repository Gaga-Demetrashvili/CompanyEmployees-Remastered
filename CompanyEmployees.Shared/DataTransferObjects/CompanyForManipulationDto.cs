namespace CompanyEmployees.Shared.DataTransferObjects;

public record CompanyForManipulationDto
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? Country { get; init; }
    public IEnumerable<EmployeeForCreationDto> Employees { get; init; } = [];
}
