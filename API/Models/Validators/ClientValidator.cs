using API.Models.DTO;
using FluentValidation;

namespace API.Models.Validators
{
    public class ClientValidator : AbstractValidator<PostClientDTO>
    {
        public ClientValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                                .MaximumLength(250).WithMessage("Length must be less than 250. Current value is {PropertyValue}");

            RuleSet("Login", () =>
            {
                RuleFor(c => c.Email).NotEmpty()/*.When(c => c.ClientType != Enums.ClientTypeEnum.Employee)*/.WithMessage("{PropertyName} is required.");
                RuleFor(c => c.Password).NotEmpty()/*.When(c => c.ClientType != Enums.ClientTypeEnum.Employee)*/.WithMessage("{PropertyName} is required.");
            });

            //RuleFor(c => c.ClientType).NotNull().WithMessage("{PropertyName} is required.")
            //    .IsInEnum().WithMessage("{PropertyName} invalid.");
        }
    }
}
