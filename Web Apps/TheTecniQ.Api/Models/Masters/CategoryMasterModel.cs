using FluentValidation;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;

namespace TheTecniQ.API.Models.Masters
{
    public class CategoryMasterModel : BaseModel
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public decimal? SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
    public class CategoryMasterModelValidator : AbstractValidator<CategoryMasterModel>
    {
        public CategoryMasterModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required.");
        }
    }
}
