using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyEmployees.Core.Domain.Entities;

public class Employee
{
    // These attributes are here only for database validation purposes
    [Column("EmployeeId")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Employee name is a required field.")] // These attributes are here for database validation purposes
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")] // These attributes are here for database validation purposes
    public string? Name { get; set; }

    [Required(ErrorMessage = "Age is a required field.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Position is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
    public string? Position { get; set; }

    [ForeignKey(nameof(Company))]
    public Guid CompanyId { get; set; }

    // Will not be mapped as column in Db table. It is Navigational Property and serves the purpose of defining the relationship between our models.
    public Company? Company { get; set; } 
}
