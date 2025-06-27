using API.Models.DTO;
using FluentValidation;

namespace API.Models.Validators
{
    public class ClientValidator : AbstractValidator<ClientDTO>
    {
        public ClientValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Email).NotEmpty().When(c => c.ClientType != Enums.ClientTypeEnum.Employee);
            RuleFor(c => c.Password).NotEmpty().When(c => c.ClientType != Enums.ClientTypeEnum.Employee);
        }
    }
}
