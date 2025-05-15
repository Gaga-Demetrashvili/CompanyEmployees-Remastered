using CompanyEmployees.Core.Domain.Entities;

namespace CompanyEmployees.Core.Domain.Repositories;

// The Create and Delete method signatures are left synchronous. That’s because, in these methods, we are not making any changes in the database.
// All we’re doing is changing the entity’s state to Added and Deleted
public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges, CancellationToken ct = default);
    Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges, CancellationToken ct = default);
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges, CancellationToken ct = default);
    void DeleteCompany(Company company);
}