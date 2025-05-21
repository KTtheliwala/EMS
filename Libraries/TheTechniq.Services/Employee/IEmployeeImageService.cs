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
    public partial interface IEmployeeImagesService
    {
        
        Task<int> InsertAsync(tblEmployeeImage model);
        Task UpdateAsync(tblEmployeeImage model);
        Task<tblEmployeeImage> GetById(int Id);
    }
}