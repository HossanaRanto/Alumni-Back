using FluentValidation.Results;

namespace Alumni_Back.Validators
{
    public record ValidationFailed(IEnumerable<ValidationFailure> errors)
    {
        public ValidationFailed(ValidationFailure error) : this(new[] { error }) { }
        public string[] Errorsmessages
        {
            get
            {
                return this.errors.Select(error=>error.ErrorMessage).ToArray();
            }
        }
    }
}
