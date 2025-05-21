using FluentValidation;
using TheTecniQ.API.Models.Common;

namespace TheTecniQ.API.Models.Users
{
    public class UserModel : BaseModel
    {
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }        
        public string RoleName { get; set; }        
        public string FullName { get; set; }
    }
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.RoleId).NotNull().NotEmpty().WithMessage("Role name is required");
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First name is required").MaximumLength(250).WithMessage("First name maximum length 250 character is required");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last name is required").MaximumLength(250).WithMessage("Last name maximum length 250 character is required");
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("User name is required").MaximumLength(250).WithMessage("User name maximum length 250 character is required");
            //When(x => !string.IsNullOrEmpty(x.Mobile), () =>
            //{
            //    RuleFor(x => x.Mobile).MinimumLength(7).WithMessage("Please enter valid phone number.").MaximumLength(11).WithMessage("Please enter valid phone number.");
            //});
            //RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email format.");
           
        }
    }
}