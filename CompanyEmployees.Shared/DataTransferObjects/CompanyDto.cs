namespace CompanyEmployees.Shared.DataTransferObjects;

// A Record type provides an easier way to create an immutable reference type in .NET.
// This means that the Record’s instance property values cannot change after its initialization.
// The data are passed by value, and the equality between the two records is verified by comparing the values of their properties.

// Records can be a valid alternative to classes when we have to send or receive data.
// The very purpose of a DTO is to transfer data from one part of the code to another, and immutability in many cases is useful.
// We use them to return data from a Web API or to represent events in our application.

// public record CompanyDto(Guid Id, string Name, string FullAddress);

// In order to implement content negotiation feature for example for returning xml response properly,
// instead of using positional record we have to use nomianl record.

// Positional record can be also serialized to xml with [Serializable] attribute but it is not recommended, because under the hood
// compiler transforms it as class and then adds names to properties and property names become long and not very useful.

public record CompanyDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? FullAddress { get; init; }
}

// This object is still immutable and init-only properties protect the state of the object from mutation once initialization is finished.
// We also removed the [Serializable] attribute.
