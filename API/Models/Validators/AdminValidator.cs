using API.Models.DTO;
using FluentValidation;

namespace API.Models.Validators
{
    public class AdminValidator : AbstractValidator<PostUserDTO>
    {
        public AdminValidator()
        {
            RuleFor(a => a.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                                .MaximumLength(250).WithMessage("Length must be less than 250. Current value is {PropertyValue}");
            RuleFor(a => a.Email).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(a => a.Password).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleForEach(a => a.CompanyId).NotNull().WithMessage("{PropertyName} is required.")
                                         .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}
