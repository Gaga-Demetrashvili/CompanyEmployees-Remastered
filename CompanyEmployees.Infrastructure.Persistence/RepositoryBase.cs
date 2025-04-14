using CompanyEmployees.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CompanyEmployees.Infrastructure.Persistence;

//  This abstract class, as well as the IRepositoryBase interface, work with the generic type T.
//  This type T gives even more reusability to the RepositoryBase class.
//  That means we don’t have to specify the exact model(class) right now for the RepositoryBase to work with.
//  We can do that later on.

//  Moreover, we can see the trackChanges parameter.
//  We are going to use it to improve our read-only query performance.
//  When it’s set to false, we attach the AsNoTracking method to our query to inform EF Core that it doesn’t need to track changes for the required entities.
//  This dramatically improves the speed of a query.
public class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    protected RepositoryContext RepositoryContext;

    public RepositoryBase(RepositoryContext repositoryContext) =>
        RepositoryContext = repositoryContext;

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
            RepositoryContext.Set<T>()
                .AsNoTracking() :
            RepositoryContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
            RepositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking() :
            RepositoryContext.Set<T>()
                .Where(expression);

    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
}
