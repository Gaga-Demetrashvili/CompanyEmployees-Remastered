using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;

namespace CompanyEmployees.Infrastructure.Persistence.Repositories;

//  We changed the signature of the class to internal sealed to limit the access to this class and also be explicit that it can’t be inherited from it.
internal sealed class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    // ToList() Executes the query against the database immediately and brings all the results into memory as a list.
    // Once you call .ToList(), any filtering/sorting afterward is done in-memory, not in the DB.
    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();
}