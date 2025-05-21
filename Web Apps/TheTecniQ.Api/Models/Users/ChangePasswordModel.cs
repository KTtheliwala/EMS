using FluentValidation;

namespace TheTecniQ.API.Models.Users
{
    public class ChangePasswordModel
    {
        public string Oldpassword { get; set; }
        public string Newpassword { get; set; }
        public string Confirmpassword { get; set; }
    }
    public class ChangePasswordModellValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModellValidator()
        {
            RuleFor(x => x.Oldpassword).NotNull().NotEmpty().WithMessage("Old password is required");
            RuleFor(x => x.Newpassword).NotNull().NotEmpty().WithMessage("New password is required")
            .Matches(@"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$").WithMessage("New password must be 8 characters or longer with 1 lowercase,1 uppercase,1 numeric & 1 special character.");
            //RuleFor(x => x.Newpassword).NotNull().NotEmpty().WithMessage("New password is required").MinimumLength(5).WithMessage("New Password minimum 5 character is required");
            RuleFor(x => x.Confirmpassword).NotNull().NotEmpty().WithMessage("Confirm password is required").MinimumLength(8).WithMessage("Confirm Password minimum 8 character is required");
            RuleFor(x => x.Newpassword).Equal(x => x.Confirmpassword).WithMessage("New Password and confirmation password does not match.");
        }
    }
}
