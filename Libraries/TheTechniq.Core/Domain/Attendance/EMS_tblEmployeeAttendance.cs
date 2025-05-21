using System;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Attendance
{
    public class EMS_tblEmployeeAttendance : BaseEntity
    {
        public int EmployeeID { get; set; }
        [NoColumnMap]
        public int? SrNo { get; set; }

        [NoColumnMap]  // Still prevents database mapping
        public int? _voucherNo;
        [NoColumnMap]

        public int? VoucherNo
        {
            get => _voucherNo ?? (Id > 0 ? Id : null);  // Default to Id if null
            set => _voucherNo = value; // Allow manual binding
        }
        public DateTime AttendanceDate { get; set; }
        public decimal? AttendDays { get; set; }
        public decimal? TotalExpenseAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalExtraAmount { get; set; }
        public string SalaryType { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? PayableAmount { get; set; }
        public string DivisionName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        [NoColumnMap]
        public string EmployeeName { get; set; }

        [NoColumnMap]
        public int? EnrollNo { get; set; }

        [NoColumnMap]
        public string AdharcardNo { get; set; }

        [NoColumnMap]
        public string rowClass { get; set; } = "";
    }
}
