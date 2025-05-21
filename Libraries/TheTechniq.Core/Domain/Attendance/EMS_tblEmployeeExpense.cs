using System;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Attendance
{
    public class EMS_tblEmployeeExpense : BaseEntity
    {
        public int EmployeeID { get; set; }
        [AuditLog(IsIgnoreTimeZone = true, ExportFormat = "mmm-yyyy")]
        public DateTime ExpenseDate { get; set; }
        public int? ExpenseNo { get; set; }
        public string ExpenseMonth { get; set; }
        public string Remark { get; set; }
        public decimal? Amount { get; set; }
        public int CreatedBy { get; set; }
        [AuditLog(IsIgnoreTimeZone = false, ExportFormat = "dd MMM yyyy h:mm tt")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        [NoColumnMap]
        public string EmployeeName { get; set; }

        [NoColumnMap]
        public int? SrNo { get; set; }
        public int? EnrollNo { get; set; }
        [NoColumnMap]
        public string SalaryType { get; set; }

        [NoColumnMap]
        public string AdharcardNo { get; set; }
        [NoColumnMap]
        public string DivisionName { get; set; }
        [NoColumnMap]
        public string DesignationName { get; set; }
        [NoColumnMap]
        public string DepartmentName { get; set; }
        [NoColumnMap]
        public decimal? BasicSalary { get; set; }

    }
}
