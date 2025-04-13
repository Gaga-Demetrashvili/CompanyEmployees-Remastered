using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyEmployees.Core.Domain.Entities;

//  You can also see the ErrorMessage properties for the Required and MaxLength attributes.
//  We added them here so you can see that you can provide these custom error messages for each column and the validation rule.
//  However, as you will see in future modules, we will use DTOs to accept parameters from the client, validate them, and return results to the client. 
//  Those DTOs will also have their validation attributes with custom error messages. 
//  Thus, these custom error messages in the model classes don’t provide us much value.

//  We will use these two models only to communicate with the database through a repository pattern and create migrations, 
//  and validation messages won’t play any role there.Therefore, we can omit them.

//  If you are using these validation attributes only for repository methods and entities for a database, 
//  the validation error messages will not be automatically triggered by Entity Framework(EF) during database operations.
//  EF Core does not perform validation based on data annotations when saving entities to the database.

//  If you are using these attributes only for database entities and repository methods:
//  •	They will enforce database schema constraints but will not automatically validate data before saving. So they are only for DB in this case.
//  •	You need to manually validate the entities in your application logic (e.g., in repository methods) if you want to use the error messages.

//  MOST IMPORTANT!!! So this attributes for example Required are only used for DB validations during DB schema migrations for example making that column NOT NULL.
//  In that case if I insert null value in DB, I'll get an error from DB.

// If you are only using the Company and Employee classes as database entities and not for direct validation
// in your application (e.g., via manual validation or model binding in ASP.NET Core),
// then the custom error messages in the [Required] and [MaxLength] attributes do not provide much value.
// In case you insert more than 60 characters you will get different, DB specific error message from DB
// and not the one you provided in the attribute (ErrorMessage = "Maximum length for the Name is 60 characters in case of Name for example.

// IMPORTANT!!! Error Messages are not needed, but attributes for sure are. these attributes force validation rules in DBs.
// So they are needed if you use EF migrations for DB creation and if you are not using fluent validation.
public class Company
{
    // These attributes are here only for database validation purposes
    [Column("CompanyId")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Company name is a required field.")] // These attributes are here for database validation purposes
    [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")] // These attributes are here for database validation purposes
    public string? Name { get; set; }

    [Required(ErrorMessage = "Company address is a required field.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters")]
    public string? Address { get; set; }

    public string? Country { get; set; }

    // Will not be mapped as column in Db table. It is Navigational Property and serves the purpose of defining the relationship between our models.
    public ICollection<Employee>? Employees { get; set; }
}
