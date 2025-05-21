using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Attendance;
using TheTecniQ.Core.Domain.Employees;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Services.Common;

namespace TheTecniQ.Services.Attendance
{
    public partial interface IActivationService
    {
        Task<IList<DropdDownEmployeeModel>> EmployeeSearch(SearchEmployeeModel search);
        Task<IPagedList<EMS_tblAttendanceActivation>> GetAllAsync(GridRequestModel objGrid);
        
    }
}