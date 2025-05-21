using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.API.Models.Orders
{
    public class OrderHistoryMasterModel : BaseModel
    {
        public int OrderId { get; set; }
        public int OrderStatus { get; set; }
        public string Remarks { get; set; }
    }
    public class OrderHistoryMasterModelValidator : AbstractValidator<OrderHistoryMasterModel>
    {
        public OrderHistoryMasterModelValidator()
        {
            RuleFor(x => x.OrderId).NotNull().NotEmpty().WithMessage("Order is required.");
            RuleFor(x => x.OrderStatus).NotNull().NotEmpty().WithMessage("Status is required.");
        }
    }
}
