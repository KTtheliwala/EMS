using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Employees
{
    public class tblEmployeeImage : BaseEntity
    {
        public int ImageNewID { get; set; }
        public int? EmployeeID { get; set; }
        public byte[] ImageEmployeePhoto { get; set; }
        public byte[] ImageAadharCardFront { get; set; }
        public byte[] ImageAadharCardBack { get; set; }
        public byte[] ImageOtherDoc { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }        
    }
}
