using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;

namespace TheTecniQ.API.Models.Expense
{
    public class EMS_tblEmployeeExpenseModel : BaseModel
    {
        public int EmployeeID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseMonth { get; set; }
        public int ExpenseNo { get; set; }
        public decimal? Amount { get; set; }
        public string Remark { get; set; }
        public int? EnrollNo { get; set; }
    }
    public class EMS_tblEmployeeExpenseModelValidator : AbstractValidator<EMS_tblEmployeeExpenseModel>
    {
        public EMS_tblEmployeeExpenseModelValidator()
        {
            RuleFor(x => x.EmployeeID).NotNull().NotEmpty().WithMessage("Employee is required.");
            //RuleFor(x => x.EnrollNo).NotNull().NotEmpty().WithMessage("EnrollNo is required.");
            RuleFor(x => x.ExpenseDate).NotNull().NotEmpty().WithMessage("Date is required.");           
        }
    }

    public class EmployeeActivationCheck {
        public int EmployeeID { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
