using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Shared.DataTransferObjects;
using CompanyEmployees.Shared.RequestFeatures;
using LoggingService;
using System.Dynamic;

namespace CompanyEmployees.Core.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<EmployeeDto> _dataShaper;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges, CancellationToken ct = default)
    {
        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repository.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges, CancellationToken ct = default)
    {
        var company = await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeForCompany = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges, ct);

        using var transaction = _repository.BeginTransaction();

        _repository.Employee.DeleteEmployee(company, employeeForCompany);

        transaction.Commit();
    }

    // var company = FindByCondition(c => c.Id.Equals(companyId), trackChanges)
    //    .Include(c => c.Employees)
    //    .Select(c => new Company
    //    {
    //        Id = c.Id,
    //        Name = c.Name,
    //        Employees = c.Employees.Select(e => new Employee { Id = e.Id, Name = e.Name }).ToList()
    //    }).Single();

    // Here, we use the FindByCondition() method to get a single company, include all the employees for that company
    // And then use the Select() method to project the result creating the Company and Employee records with only required columns (properties).

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges, CancellationToken ct = default)
    {
        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeeDb = GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges, ct);

        var employeeDto = _mapper.Map<EmployeeDto>(employeeDb);

        return employeeDto;
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges, CancellationToken ct = default)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges, ct);

        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges, ct);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        return (employeeToPatch, employeeEntity);
    }

    public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges, CancellationToken ct = default)
    {
        if (!employeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId, trackChanges, ct);

        var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges, ct);

        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
        var shapedData = _dataShaper.ShapeData(employeesDto, employeeParameters.Fields!);

        return (employees: shapedData, metaData: employeesWithMetaData.MetaData);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges, CancellationToken ct = default)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges, ct);

        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges, ct);

        _mapper.Map(employeeForUpdate, employeeEntity);
        await _repository.SaveAsync();
    }

    private async Task<Company> CheckIfCompanyExists(Guid companyId, bool trackChanges, CancellationToken ct) =>
        await _repository.Company.GetCompanyAsync(companyId, trackChanges, ct) ??
        throw new CompanyNotFoundException(companyId);

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges, CancellationToken ct) =>
        await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges, ct) ??
        throw new EmployeeNotFoundException(id);
}