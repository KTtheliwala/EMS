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
    public partial class DesignationService() : IDesignationService
    {
        #region Fields
        #endregion



        #region Methods

        #region Get

        public async Task<IList<tblDesignation>> GetAll(int? departmentId= null)
        {
            MsSqlDataProvider objSql = new();
            string sqlQuery = @"select * from tblDesignation Order by DesignationName";
            if (departmentId != null && departmentId > 0)
            {
                var existingdesignation = await objSql.QueryAsync<int>(@"select  DesignationID from tblDeptDesigationAutoGenerate where DepartmentID = "+ departmentId, null);
                if(existingdesignation != null && existingdesignation.Count > 0)
                    sqlQuery = @"select * from tblDesignation Where DesignationID in ("+ string.Join(",",existingdesignation)+ ") Order by DesignationName";
                else
                    sqlQuery = @"select * from tblDesignation Where DesignationID in (0) Order by DesignationName";
            }
            
            return await objSql.QueryAsync<tblDesignation>(sqlQuery, null);            
        }
        
        #endregion

        #endregion
    }
}