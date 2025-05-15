using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Infrastructure.Persistence.Repositories;

//  We changed the signature of the class to internal sealed to limit the access to this class and also be explicit that it can’t be inherited from it.
internal sealed class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public void CreateCompany(Company company) => Create(company);

    public void DeleteCompany(Company company) => Delete(company);

    // ToList() Executes the query against the database immediately and brings all the results into memory as a list.
    // Once you call .ToList(), any filtering/sorting afterward is done in-memory, not in the DB.
    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges, CancellationToken ct = default) =>
        await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges, CancellationToken ct = default) =>
        await FindByCondition(c => ids.Contains(c.Id), trackChanges)
        .ToListAsync();

    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default) =>
       (await FindByCondition(c => c.Id.Equals(companyId), trackChanges)
        .SingleOrDefaultAsync())!;
}