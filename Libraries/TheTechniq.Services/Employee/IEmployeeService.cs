using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Employees;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Services.Common;

namespace TheTecniQ.Services.Employees
{
    public partial interface IEmployeeService
    {
        Task<IPagedList<tblEmployeeDto>> GetAllAsync(GridRequestModel objGrid);
        Task<int> InsertAsync(tblEmployee model);
        Task UpdateAsync(tblEmployee model);
        Task<tblEmployee> GetById(int Id);
        Task<bool> CheckEnrollNo(string EnrollNo, int? Id);
        Task<IList<tblEmployee>> GetByIds(IList<int> ids);
    }
}