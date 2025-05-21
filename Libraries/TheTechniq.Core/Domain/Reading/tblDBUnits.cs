using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTecniQ.Core.Domain.Reading
{
    public class tblDBUnits
    {
        public int DBUnitID { get; set; }
        public int? DBUnitNo { get; set; }
        public DateTime DBUnitDate { get; set; }
        public int? DBID { get; set; }
        public float? Units { get; set; }
        public float? UsedUnits { get; set; }
        public float? PREVUNITS { get; set; }
        public string DBName { get; set; }
    }

}
