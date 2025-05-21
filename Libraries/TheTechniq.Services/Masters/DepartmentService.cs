using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Data;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Services.Common;
using TheTecniQ.Core.Configuration;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Core.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.Xml;
using System.Data;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Data.DataProviders;
using LinqToDB.Data;
using TheTecniQ.Services.Users;
using TheTecniQ.Core.Domain.Masters;

namespace TheTecniQ.Services.Masters
{
    public partial class DepartmentService() : IDepartmentService
    {
        #region Fields
        #endregion



        #region Methods

        #region Get

        public async Task<IList<tblDepartment>> GetAll()
        {
            MsSqlDataProvider objSql = new();            
            return await objSql.QueryAsync<tblDepartment>(@"select * from tblDepartment Order by DepartmentName", null);            
        }
        
        #endregion

        #endregion
    }
}