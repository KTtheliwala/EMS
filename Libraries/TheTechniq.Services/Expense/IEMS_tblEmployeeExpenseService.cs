using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Attendance;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Services.Common;

namespace TheTecniQ.Services.Expense
{
    public partial interface IEMS_tblEmployeeExpenseService : ICommonService<EMS_tblEmployeeExpense>
    {
        new Task<IPagedList<EMS_tblEmployeeExpense>> GetAllAsync(GridRequestModel objGrid);
        Task<bool> CheckEmployeeActivation(int EmployeeId, DateTime dt);
        Task<int> GetMaxExpenseNo();
        Task<IList<EMS_tblEmployeeExpense>> GetAllAsync_Rpt(DateTime startdate, DateTime endDate, string enrollNo);
        Task<IList<EMS_tblEmployeeExpense>> getExistingExpense(int EmployeeId, DateTime dt);
        Task<IList<EMS_tblEmployeeExpense>> getExpenseList(IList<int> EmployeeIds);
    }
}