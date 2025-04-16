using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;

namespace CompanyEmployees.Core.Services;

public class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repositorყ;
    private readonly ILoggerManager _logger;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repositorყ = repository;
        _logger = logger;
    }
}