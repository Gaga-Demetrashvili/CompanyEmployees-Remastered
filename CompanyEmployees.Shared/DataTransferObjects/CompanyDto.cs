namespace CompanyEmployees.Shared.DataTransferObjects;

// A Record type provides an easier way to create an immutable reference type in .NET.
// This means that the Record’s instance property values cannot change after its initialization.
// The data are passed by value, and the equality between the two records is verified by comparing the values of their properties.

// Records can be a valid alternative to classes when we have to send or receive data.
// The very purpose of a DTO is to transfer data from one part of the code to another, and immutability in many cases is useful.
// We use them to return data from a Web API or to represent events in our application.
public record CompanyDto(Guid Id, string Name, string FullAddress);
