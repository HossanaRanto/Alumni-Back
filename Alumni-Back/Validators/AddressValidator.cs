using Alumni_Back.DTO;
using FluentValidation;

namespace Alumni_Back.Validators
{
    public class AddressValidator:AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.country).NotEmpty().WithMessage("Country required");
            RuleFor(x => x.city).NotEmpty().WithMessage("City Required");
            RuleFor(x => x.postalcode).NotEmpty().WithMessage("Postal code required");
        }
    }
}
