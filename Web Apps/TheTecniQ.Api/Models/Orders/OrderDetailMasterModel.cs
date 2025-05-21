using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.API.Models.Orders
{
    public class OrderDetailMasterModel : BaseModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public int CreatedBy { get; set; }
        [NoColumnMap]
        public string CreatedUserName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
    public class OrderDetailMasterModelValidator : AbstractValidator<OrderDetailMasterModel>
    {
        public OrderDetailMasterModelValidator()
        {
            RuleFor(x => x.OrderId).NotNull().NotEmpty().WithMessage("Order is required.");
            RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage("Product is required.");
            RuleFor(x => x.Quantity).NotNull().NotEmpty().GreaterThan(0).WithMessage("Quantity is required and greater then 0.");            
        }
    }
}
