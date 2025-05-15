using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CompanyEmployees.Infrastructure.Persistence;

// The exciting part about the RepositoryManager implementation is that we are leveraging the power of
// the Lazy class to ensure the lazy initialization of our repositories.
// This means that our repository instances will only be created when we access them for the first time, not before that.
public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(_repositoryContext));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_repositoryContext));
    }

    public ICompanyRepository Company => _companyRepository.Value;

    public IEmployeeRepository Employee => _employeeRepository.Value;

    public async Task SaveAsync(CancellationToken ct = default) => await _repositoryContext.SaveChangesAsync(ct);

    public IDbContextTransaction BeginTransaction() => _repositoryContext.Database.BeginTransaction();
}