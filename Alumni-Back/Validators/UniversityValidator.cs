using Alumni_Back.DTO;
using FluentValidation;

namespace Alumni_Back.Validators
{
    public class UniversityValidator:AbstractValidator<UniversityDto>
    {
        public UniversityValidator()
        {
            RuleFor(x => x.Contact).MinimumLength(12).WithMessage("Phone number invalid");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email address invalid");
            RuleFor(x => x.Address).Must(ValidAddress).WithMessage("Address invalid");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Add a description");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name required");
        }
        protected bool ValidAddress(AddressDto address)
        {
            var addressvalidator = new AddressValidator();
            return addressvalidator.Validate(address).IsValid;
        }
    }
}
