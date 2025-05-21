using System;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Attendance
{
    public class EMS_tblAttendanceActivation:BaseEntity
    {
        public int EmployeeID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public decimal? AttendDays { get; set; }
        public bool IsActive { get; set; } = false;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        [NoColumnMap]
        public string EmployeeName { get; set; }

        [NoColumnMap]
        public int? EnrollNo { get; set; }

    }
}
