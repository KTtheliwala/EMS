using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.API.Models.Masters
{
    public class ProductMasterModel : BaseModel
    {
        public string Name { get; set; }
        public string PlainTextName { get; set; }
        public int CategoryId { get; set; }
        public string ProductCode { get; set; }
        public string ProdutSlug { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public bool IsNewProduct { get; set; } = false;
        public bool TopSellingProduct { get; set; } = false;
        public bool IsOfferApplicable { get; set; } = false;
        public decimal? Price { get; set; }
        public string OfferText { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public decimal? SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
    }
    public class ProductMasterModelValidator : AbstractValidator<ProductMasterModel>
    {
        public ProductMasterModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.ProductCode).NotNull().NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.CategoryId).NotNull().NotEmpty().WithMessage("Category is required.");
        }
    }    
}
