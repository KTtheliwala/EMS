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
    public partial interface IAttendanceService
    {
        Task<IList<DropdDownEmployeeModel>> ActiveEmployeeSearch(SearchActiveEmployeeModel search);
        Task<IPagedList<EMS_tblEmployeeAttendance>> GetAllAsync(GridRequestModel objGrid);
        Task<IList<EMS_tblEmployeeAttendance>> GetAllAsync_Rpt(GridRequestModel objGrid);
        Task<EMS_tblEmployeeAttendance> GetEmployeeAttendence(int employeeId, DateTime dt);
        Task<decimal?> GetEmployeeExpense(int employeeId, DateTime dt);
    }
}