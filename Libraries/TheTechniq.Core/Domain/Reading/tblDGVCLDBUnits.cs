using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Common;

namespace TheTecniQ.Core.Domain.Reading
{
    public class tblDGVCLDBUnits
    {
        public int DBDGVCLUnitID { get; set; }
        public int DBDGVCLUnitNo { get; set; }
        public DateTime DBDGVCLUnitDate { get; set; }
        public int? DBPanelID { get; set; }
        public float? KWH { get; set; }
        public float? KVAH { get; set; }
        public float? KVARH { get; set; }
        public float? KWHDiff { get; set; }
        public float? KVAHDiff { get; set; }
        public float? PF { get; set; }
        public float? Units { get; set; }
        public int? CRBy { get; set; }
        public int? UpBy { get; set; }
        public DateTime? CRDate { get; set; }
        public DateTime? UpdatedDate { get; set; }        
        public string DBName { get; set; }
    }
}
