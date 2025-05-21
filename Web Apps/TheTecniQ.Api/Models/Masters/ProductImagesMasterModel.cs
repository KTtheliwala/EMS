using FluentValidation;
using Microsoft.AspNetCore.Http;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.API.Models.Masters
{
    public class ProductImagesMasterModel : BaseModel
    {
        public string EmployeeName { get; set; }
        public string FileName { get; set; }
        public int ProductId { get; set; }
        public string AltTitle { get; set; }
        public string AltKeyword { get; set; }
        public string AltDescription { get; set; }
        public decimal? SortOrder { get; set; }
        public bool? IsThumbnail { get; set; } = false;
        public IFormFile ImageAadharCardFront { get; set; }
    }
    public class ProductImagesMasterModelValidator : AbstractValidator<ProductImagesMasterModel>
    {
        public ProductImagesMasterModelValidator()
        {
            //RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage("Product is required.");
            //RuleFor(x => x.ImageAadharCardFront).NotNull().NotEmpty().WithMessage("File is required.").When(x => x.Id == 0); 
        }
    }
}
