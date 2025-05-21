using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;

namespace TheTecniQ.API.Models.Expense
{
    public class EMS_tblEmployeeAttendanceModel : BaseModel
    {
        public int EmployeeID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public decimal? AttendDays { get; set; }
        public decimal? TotalExpenseAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Remarks { get; set; }
        public decimal? TotalExtraAmount { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? PayableAmount { get; set; }
        public string SalaryType { get; set; }
        public string DivisionName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
    }
    public class EMS_tblEmployeeAttendanceModelValidator : AbstractValidator<EMS_tblEmployeeAttendanceModel>
    {
        public EMS_tblEmployeeAttendanceModelValidator()
        {
            RuleFor(x => x.EmployeeID).NotNull().NotEmpty().WithMessage("Employee is required.");            
            RuleFor(x => x.AttendanceDate).NotNull().NotEmpty().WithMessage("Month-Year is required.");           
            RuleFor(x => x.AttendDays).NotNull().NotEmpty().WithMessage("Attend Days is required.");
            RuleFor(x => x.TotalAmount)
     .NotNull().WithMessage("Total amount is required.")
     .NotEmpty().WithMessage("Total amount cannot be empty.")
     .GreaterThan(0).WithMessage("Total amount must be greater than zero.");

        }
    }
}
