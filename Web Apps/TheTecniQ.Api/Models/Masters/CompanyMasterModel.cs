using FluentValidation;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;

namespace TheTecniQ.API.Models.Masters
{
    public class CompanyMasterModel : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public decimal? SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
    public class CompanyMasterModelValidator : AbstractValidator<CompanyMasterModel>
    {
        public CompanyMasterModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email is required.");
        }
    }
}
