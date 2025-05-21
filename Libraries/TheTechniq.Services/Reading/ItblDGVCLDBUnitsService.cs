using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Employees;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Masters;
using TheTecniQ.Core.Domain.Reading;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Services.Common;

namespace TheTecniQ.Services.Reading
{
    public partial interface ItblDGVCLDBUnitsService
    {
        Task<int> InsertAsync(tblDGVCLDBUnits model);        
        Task<bool> CheckAlreadyExist(tblDGVCLDBUnits model);
        Task<IList<tblDBMaster>> getTblDBUnits();
        Task<IList<tblDBPanelMaster>> getTblDBPanelMaster();
        Task<bool> CheckAlreadyExist(tblDBUnits model);
        Task<int> InsertAsync(tblDBUnits model);
        Task<IList<tblDBUnits>> ListTblDBUnits(DateTime dt);
        Task<IList<tblDGVCLDBUnits>> ListTblDGVCLDBUnits(DateTime dt);
    }
}