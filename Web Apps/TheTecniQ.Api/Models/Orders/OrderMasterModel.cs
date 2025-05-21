using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.API.Models.Orders
{
    public class OrderMasterModel : BaseModel
    {
        public int OrderNo { get; set; }
        public int CompanyId { get; set; }
        public DateTime? DateofDelivery { get; set; }
        public DateTime? DateOfFunction { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public int? CountryId { get; set; }
        public int OrderStatus { get; set; }
        public string Remarks { get; set; }
        public int? DiscountTypeId { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        public List<OrderDetailMasterModel> OrderDetails{ get; set; }
}
    public class OrderMasterModelValidator : AbstractValidator<OrderMasterModel>
    {
        public OrderMasterModelValidator()
        {
            RuleFor(x => x.CompanyId).NotNull().NotEmpty().WithMessage("Company is required.");
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().WithMessage("Delivery address is required.");
            RuleFor(x => x.ContactEmail).NotNull().NotEmpty().WithMessage("Contact email is required.");
            RuleFor(x => x.CountryId).NotNull().NotEmpty().WithMessage("Country is required.");
            RuleFor(x => x.City).NotNull().NotEmpty().WithMessage("City is required.");
            //RuleFor(x => x.State).NotNull().NotEmpty().WithMessage("State is required.");
            RuleFor(x => x.PostCode).NotNull().NotEmpty().WithMessage("Postal code is required.");
            RuleFor(x => x.OrderDetails).NotNull().NotEmpty().WithMessage("Order details is required.");
            RuleFor(x => x.SubTotalAmount).NotNull().NotEmpty().GreaterThan(0).WithMessage("Subtotal amount is required.");
            RuleFor(x => x.TotalAmount).NotNull().NotEmpty().GreaterThan(0).WithMessage("Total amount is required.");
        }
    }
}
