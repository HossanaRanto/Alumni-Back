using Alumni_Back.DTO;
using FluentValidation;

namespace Alumni_Back.Validators
{
    public class UserValidator:AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("The password should contains at least 8 digits");
            RuleFor(x => x.Birthdate).Must(ValidAge).NotEmpty().WithMessage("A student need to be over 13 years old");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email address invalid");
            RuleFor(x => x.Contact).MinimumLength(7).MaximumLength(15).WithMessage("Phone number invalid");
            RuleFor(x => x.Firstname).NotEmpty().WithMessage("First name required");
            RuleFor(x => x.Address).Must(ValidAddress).WithMessage("Address invalid");
        }
        protected bool ValidAge(DateTime birthdate)
        {
            var now=DateTime.Now;
            return now.Year - birthdate.Year >= 13;
        }
        protected bool ValidAddress(AddressDto address)
        {
            var addressvalidator=new AddressValidator();
            return addressvalidator.Validate(address).IsValid;
        }
    }
}
