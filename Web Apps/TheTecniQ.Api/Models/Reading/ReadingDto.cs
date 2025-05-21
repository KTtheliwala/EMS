using System;
using System.Collections.Generic;
using TheTecniQ.Core.Domain.Reading;

namespace TheTecniQ.Api.Models.Reading
{
    public class ReadingDto
    {
        public IList<tblDGVCLDBUnits> panelReading { get; set; }
        public IList<tblDBUnits> dbReading { get; set; }
    }
}
