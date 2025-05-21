using FluentValidation;
using TheTecniQ.Core.Infrastructure;

namespace TheTecniQ.Api.Models.Login
{
    public class AuthenticateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Confirm { get; set; } = false;
        public string CaptchaCode { get; set; }
        public string CaptchaToken { get; set; }
        public string Platform { get; set; } = PlatForm.Desktop.ToDescription();
    }
    public class AuthenticateWith2FAModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }

    public class ADAuthenticateModel
    {
        public string Token { get; set; }
    }
    public class RefreshAuthenticateModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
        public string CaptchaCode { get; set; }
        public string CaptchaToken { get; set; }
        public string Platform { get; set; } = PlatForm.Desktop.ToDescription();
    }
    public class AuthenticateModelValidator : AbstractValidator<AuthenticateModel>
    {
        public AuthenticateModelValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("User Name is required.");
            //RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password is required");
        }
    }
    public class RefreshAuthenticateModelValidator : AbstractValidator<RefreshAuthenticateModel>
    {
        public RefreshAuthenticateModelValidator()
        {
            RuleFor(x => x.Token).NotNull().NotEmpty().WithMessage("Access token is required.");
            RuleFor(x => x.RefreshToken).NotNull().NotEmpty().WithMessage("Refresh token is required.");
        }
    }
    public class ForgotPasswordModelValidator : AbstractValidator<ForgotPasswordModel>
    {
        public ForgotPasswordModelValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Username is required"); //.EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.CaptchaCode).NotNull().NotEmpty().WithMessage("Captcha code is required.");
            RuleFor(x => x.CaptchaToken).NotNull().NotEmpty().WithMessage("Captcha token is required.");
        }
    }
}
