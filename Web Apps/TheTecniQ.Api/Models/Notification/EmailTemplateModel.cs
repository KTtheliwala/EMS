using FluentValidation;
using TheTecniQ.API.Models.Common;

namespace TheTecniQ.API.Models.Notification
{
    public class EmailTemplateModel : BaseModel
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateBody { get; set; }
        public bool IsActive { get; set; }
    }
    public class EmailTemplateModelValidator : AbstractValidator<EmailTemplateModel>
    {
        public EmailTemplateModelValidator()
        {
            RuleFor(x => x.FromEmail).NotNull().NotEmpty().WithMessage("From email is required.");
            RuleFor(x => x.FromName).NotNull().NotEmpty().WithMessage("From name is required.");
            RuleFor(x => x.TemplateCode).NotNull().NotEmpty().WithMessage("Template code is required.");
            RuleFor(x => x.TemplateSubject).NotNull().NotEmpty().WithMessage("Template subject is required.");
            RuleFor(x => x.TemplateBody).NotNull().NotEmpty().WithMessage("Template body is required.");
        }
    }
}
