using CompanyEmployees.Core.Domain.Entities;
using System.Dynamic;

namespace CompanyEmployees.Core.Services.Abstractions;

public interface IDataShaper<T>
{
    IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);
    ShapedEntity ShapeData(T entity, string fieldsString);
}