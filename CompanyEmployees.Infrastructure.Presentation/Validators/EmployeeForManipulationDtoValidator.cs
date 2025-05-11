using CompanyEmployees.Shared.DataTransferObjects;
using FluentValidation;

namespace CompanyEmployees.Infrastructure.Presentation.Validators;

public abstract class EmployeeForManipulationDtoValidator<T> : AbstractValidator<T> where T : CompanyForManipulationDto
{
    public EmployeeForManipulationDtoValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Company name is a required field.")
            .MaximumLength(30).WithMessage("Maximum length for the Name is 30 characters.");

        RuleFor(e => e.Address)
            .NotEmpty().WithMessage("Address is a required field.")
            .MaximumLength(30).WithMessage("Maximum length for the Address is 30 characters.");

        RuleFor(e => e.Country)
            .NotEmpty().WithMessage("Country is a required field.")
            .MaximumLength(20).WithMessage("Maximum length for the Country is 20 characters.");
    }
}