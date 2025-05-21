using FluentValidation;

namespace TheTecniQ.API.Models.Users
{
    public class ResetPasswordModel
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string EncryptedId { get; set; }
    }
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(x => x.NewPassword).NotNull().NotEmpty().WithMessage("New Password is required")
             .Matches(@"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$").WithMessage("New Password must be 8 characters or longer with 1 lowercase,1 uppercase,1 numeric & 1 special character.");
            RuleFor(x => x.ConfirmPassword).NotNull().NotEmpty().WithMessage("Confirmation Password is required");
            RuleFor(x => x.NewPassword).Equal(x => x.ConfirmPassword).WithMessage("Password and confirmation Password does not match");
        }
    }
}