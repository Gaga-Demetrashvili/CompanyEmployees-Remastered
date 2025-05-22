namespace CompanyEmployees.Core.Domain.Entities;

public class ShapedEntity
{
    public ShapedEntity()
    {
        Entity = new Entity();
    }
    public Guid Id { get; set; }
    public Entity Entity { get; set; }
}