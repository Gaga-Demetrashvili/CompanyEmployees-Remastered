using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Infrastructure.Persistence.Extensions;
using CompanyEmployees.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    // We again save our changes and call the Commit() method using the transaction object we created at the beginning of the method.
    // Only at this point, if everything goes well inside the transaction, all the changes will be executed.

    // We don’t have to explicitly call the Restore method here because EF Core will restore all the changes automatically
    // if something goes wrong during the transaction. Again, this is why it creates those savepoints.
    public void DeleteEmployee(Company compnay, Employee employee)
    {
        // I took transaction logic out in case I'll need to start it in service layer.
        // All the database-related logic stays inside the repository classes.
        // If I don't need to use it in service layer than I do not need to add 
        // BeginTransaction Method for begining transaction in RepositoryManager class.

        // using var tranasaction = RepositoryContext.Database.BeginTransaction();

        Delete(employee);

        RepositoryContext.SaveChanges();

        if (!FindByCondition(e => e.CompanyId == compnay.Id, false).Any())
        {
            RepositoryContext.Companies!.Remove(compnay);

            RepositoryContext.SaveChanges();
        }

        // tranasaction.Commit();
    }

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges, CancellationToken ct = default) =>
        (await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync(ct))!;

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, 
        bool trackChanges, CancellationToken ct)
    {
        var employeesQuery = FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
            .Search(employeeParameters.SearchTerm)
            .Sort(employeeParameters.OrderBy!);

        var count = await employeesQuery.CountAsync(ct);

        var employees = await employeesQuery
            .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
            .Take(employeeParameters.PageSize)
            .ToListAsync(ct);

        return PagedList<Employee>
            .ToPagedList(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
    }
}