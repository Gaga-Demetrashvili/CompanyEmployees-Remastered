using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Shared.DataTransferObjects;
using LoggingService;

namespace CompanyEmployees.Core.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        _repository.Save();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
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

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeDb = _repository.Employee.GetEmployee(companyId, id, trackChanges);
        if (employeeDb is null)
            throw new EmployeeNotFoundException(id);

        var employeeDto = _mapper.Map<EmployeeDto>(employeeDb);

        return employeeDto;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeesFromDb = _repository.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        return employeesDto;
    }
}