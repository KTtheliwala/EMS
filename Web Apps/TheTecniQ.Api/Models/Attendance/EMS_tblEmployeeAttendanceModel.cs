using FluentValidation;
using System;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;

namespace TheTecniQ.API.Models.Attendance
{
    public class AttendanceActivationModel : BaseModel
    {
        public int EmployeeID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public decimal? AttendDays { get; set; }
        public bool IsActive { get; set; } = false;
    }
    public class AttendanceActivationValidator : AbstractValidator<AttendanceActivationModel>
    {
        public AttendanceActivationValidator()
        {
            RuleFor(x => x.EmployeeID).NotNull().NotEmpty().WithMessage("Employee is required.");
            RuleFor(x => x.AttendanceDate).NotNull().NotEmpty().WithMessage("Month is required.");
            //RuleFor(x => x.AttendDays).NotNull().NotEmpty().GreaterThan(0).WithMessage("Attend day(s) is required.");
        }
    }

    
}
