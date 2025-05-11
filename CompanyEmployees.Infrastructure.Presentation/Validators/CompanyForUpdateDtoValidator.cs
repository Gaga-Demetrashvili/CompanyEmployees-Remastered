using CompanyEmployees.Shared.DataTransferObjects;
using FluentValidation;

namespace CompanyEmployees.Infrastructure.Presentation.Validators;

//public class CompanyForUpdateDtoValidator : AbstractValidator<CompanyForUpdateDto>
//{
//    public CompanyForUpdateDtoValidator()
//    {
//        RuleFor(e => e.Name)
//            .NotEmpty().WithMessage("Employee name is a required field.")
//            .MaximumLength(30).WithMessage("Maximum length for the Name is 30 characters.");

//        RuleFor(e => e.Address)
//            .NotEmpty().WithMessage("Address is a required field.")
//            .MaximumLength(30).WithMessage("Maximum length for the Address is 30 characters.");

//        RuleFor(e => e.Country)
//            .NotEmpty().WithMessage("Country is a required field.")
//            .MaximumLength(20).WithMessage("Maximum length for the Country is 20 characters.");
//    }
//}

public class CompanyForUpdateDtoValidator : EmployeeForManipulationDtoValidator<CompanyForUpdateDto>
{
}