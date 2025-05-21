using FluentValidation;

namespace TheTecniQ.Api.Models.Users
{
    public class UserProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
    }
    public class UserProfileModelValidator : AbstractValidator<UserProfileModel>
    {
        public UserProfileModelValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First name is required").MaximumLength(250).WithMessage("First name maximum length 250 character is required");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last name is required").MaximumLength(250).WithMessage("Last name maximum length 250 character is required");
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("User name is required").MaximumLength(250).WithMessage("User name maximum length 250 character is required");
            When(x => !string.IsNullOrEmpty(x.Mobile), () =>
            {
                RuleFor(x => x.Mobile).MinimumLength(7).WithMessage("Please enter valid phone number.").MaximumLength(11).WithMessage("Please enter valid phone number.");
            });
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
