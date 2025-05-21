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
using EasyNetQ;
using System.Net;
using static LinqToDB.Common.Configuration;
using TheTecniQ.Core.Domain.Attendance;
using Microsoft.IdentityModel.Tokens;

namespace TheTecniQ.Services.Attendance
{
    public partial class ActivationService(IRepository<EMS_tblAttendanceActivation> AttendanceRepository) : IActivationService
    {
        #region Fields
        private readonly IRepository<EMS_tblAttendanceActivation> _AttendanceRepository = AttendanceRepository;

        #endregion



        #region Methods

        #region Get

        public async Task<IPagedList<EMS_tblAttendanceActivation>> GetAllAsync(GridRequestModel objGrid)
        {
            MsSqlDataProvider objSql = new();
            List<int> employeeId = null;
            var findDateFil = objGrid.Filters.Where(x => x.FieldName == "EnrollNo").FirstOrDefault();
            if (!string.IsNullOrEmpty(findDateFil?.FieldValue))
            {
                var employee = await objSql.QueryAsync<tblEmployee>(@"select EmployeeID from tblEmployee Where EnrollNo IN (" + findDateFil?.FieldValue.Trim() + ") ", null);
                if (employee != null && employee.Count > 0)
                {
                    employeeId = new List<int>();
                    foreach (var item in employee)
                    {                    
                        employeeId.Add(item.EmployeeID);
                    }
                }
                else
                {
                    employeeId = new List<int>();
                    employeeId.Add(0);
                }
                objGrid.Filters.Remove(findDateFil);
            }
            IQueryable<EMS_tblAttendanceActivation> query = from u in _AttendanceRepository.Table
                                                            where (employeeId == null || employeeId.Contains(u.EmployeeID))
                                                            select u;
            objSql = new();
            var data = await _AttendanceRepository.GetAllPagedAsync(objGrid, query);
            if (data != null && data.Count > 0)
            {
                var EmployeeIDList = data.Select(x => x.EmployeeID).ToList();
                if (EmployeeIDList != null && EmployeeIDList.Count > 0)
                {
                    var dataImageList = await objSql.QueryAsync<tblEmployee>(@"select * from tblEmployee Where EmployeeID IN (" + string.Join(",", EmployeeIDList) + ") ", null);
                    if (dataImageList != null && dataImageList.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            var imgObj = dataImageList.FirstOrDefault(c => c.EmployeeID == item.EmployeeID);
                            if (imgObj != null && imgObj.EmployeeID > 0)
                            {
                                item.EmployeeName = imgObj.EmployeeName;
                                item.EnrollNo = imgObj.EnrollNo;
                            }
                        }
                    }
                }
            }
            return data;

        }

        public async Task<IList<DropdDownEmployeeModel>> EmployeeSearch(SearchEmployeeModel search)
        {
            IList<DropdDownEmployeeModel> data = [];
            string whereClause = " WHERE 1=1 ";
            if (search.DepartmentID != null && search.DepartmentID > 0)
                whereClause += " AND DepartmentID=" + search.DepartmentID;
            if (search.DivisionID != null && search.DivisionID > 0)
                whereClause += " AND DivisionID=" + search.DivisionID;
            if (search.DesignationID != null && search.DesignationID > 0)
                whereClause += " AND DesignationID=" + search.DesignationID;
            if (!string.IsNullOrEmpty(search.Name))
                whereClause += " AND EmployeeName LIKE '%" + search.Name + "%'";
            if (!string.IsNullOrEmpty(search.EnrollNo))
                whereClause += " AND EnrollNo LIKE '%" + search.EnrollNo + "%'";

            MsSqlDataProvider objSql = new();
            var sqlQry = @"SELECT * FROM View_Employee " + whereClause + " ORDER BY EmployeeName";
            data = await objSql.QueryAsync<DropdDownEmployeeModel>(sqlQry, null);
            return data;
        }






        #endregion

        #endregion
    }
}