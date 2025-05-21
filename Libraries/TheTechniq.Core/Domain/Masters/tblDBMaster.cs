using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTecniQ.Core.Domain.Masters
{
    public class tblDBMaster
    {
        public int DBID { get; set; }
        public string DBName { get; set; }
        public int? CRBy { get; set; }
        public int? UpDateBy { get; set; }
        public DateTime? CRDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Remarks { get; set; }
    }
}
