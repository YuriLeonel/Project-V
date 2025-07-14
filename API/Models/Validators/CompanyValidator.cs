using API.Models.DTO;
using FluentValidation;

namespace API.Models.Validators
{
    public class CompanyValidator : AbstractValidator<PostCompanyDTO>
    {
        public CompanyValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                                .MaximumLength(250).WithMessage("Length must be less than 250. Current value is {PropertyValue}");
            RuleFor(c=>c.IdOwner).NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
