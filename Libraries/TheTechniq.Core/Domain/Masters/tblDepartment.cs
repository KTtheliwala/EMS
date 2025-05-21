using System;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Masters
{
    public class tblDepartment 
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Remarks { get; set; }
        public bool IsFreeze { get; set; } = false;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
