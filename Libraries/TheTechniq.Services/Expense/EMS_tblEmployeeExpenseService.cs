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
using TheTecniQ.Core.Domain.Attendance;
using TheTecniQ.Core.Domain.Employees;
using TheTecniQ.Data.DataProviders;

namespace TheTecniQ.Services.Expense
{
    public partial class EMS_tblEmployeeExpenseService(IRepository<EMS_tblEmployeeExpense> EMS_tblEmployeeExpenseRepository, IRepository<EMS_tblAttendanceActivation> eMS_tblAttendanceActivationRepository) : CommonService<EMS_tblEmployeeExpense>(EMS_tblEmployeeExpenseRepository), IEMS_tblEmployeeExpenseService
    {
        #region Fields
        private readonly IRepository<EMS_tblEmployeeExpense> _EMS_tblEmployeeExpenseRepository = EMS_tblEmployeeExpenseRepository;
        private readonly IRepository<EMS_tblAttendanceActivation> _eMS_tblAttendanceActivationRepository = eMS_tblAttendanceActivationRepository;
        #endregion



        #region Methods

        #region Get
        public async override Task<IPagedList<EMS_tblEmployeeExpense>> GetAllAsync(GridRequestModel objGrid)
        {
            IQueryable<EMS_tblEmployeeExpense> query = from u in _EMS_tblEmployeeExpenseRepository.Table
                                                            select u;
            MsSqlDataProvider objSql = new();
            var data = await _EMS_tblEmployeeExpenseRepository.GetAllPagedAsync(objGrid, query);
            if (data != null && data.Count > 0)
            {
                var EmployeeIDList = data.Select(x => x.EmployeeID).ToList();
                if (EmployeeIDList != null && EmployeeIDList.Count > 0)
                {
                    var dataImageList = await objSql.QueryAsync<tblEmployee>(@"select EmployeeID,EmployeeName,EnrollNo,SalaryType from tblEmployee Where EmployeeID IN (" + string.Join(",", EmployeeIDList) + ") ", null);
                    if (dataImageList != null && dataImageList.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            var imgObj = dataImageList.FirstOrDefault(c => c.EmployeeID == item.EmployeeID);
                            if (imgObj != null && imgObj.EmployeeID > 0)
                            {
                                item.EmployeeName = imgObj.EmployeeName;
                                //item.EnrollNo = imgObj.EnrollNo;
                                item.SalaryType = imgObj.SalaryType;
                            }
                        }
                    }
                }
            }
            return data;

        }
        public async Task<bool> CheckEmployeeActivation(int EmployeeId, DateTime dt)
        {
            var check = await _eMS_tblAttendanceActivationRepository.Table.Where(x => x.EmployeeID == EmployeeId && (x.AttendanceDate.Date.Month == dt.Month && x.AttendanceDate.Date.Year == dt.Year)).ToListAsync();
            return ((check.Count != null && check.Count > 0)?true:false);

        }

        public async Task<IList<EMS_tblEmployeeExpense>> getExistingExpense(int EmployeeId, DateTime dt)
        {
            return await _EMS_tblEmployeeExpenseRepository.Table.Where(x => x.EmployeeID == EmployeeId && (x.ExpenseDate.Date.Month == dt.Month && x.ExpenseDate.Date.Year == dt.Year)).ToListAsync();            

        }
        public async Task<IList<EMS_tblEmployeeExpense>> getExpenseList(IList<int> EmployeeIds)
        {
            return await _EMS_tblEmployeeExpenseRepository.Table.Where(x => EmployeeIds.Contains(x.Id)).ToListAsync();

        }
        public async Task<int> GetMaxExpenseNo()
        {
            int maxExpenseNo = await _EMS_tblEmployeeExpenseRepository.Table
         .Select(x => x.ExpenseNo ?? 0) // Replace NULL values with 0
         .DefaultIfEmpty(0) // Ensure at least one value exists
         .MaxAsync();

            return maxExpenseNo + 1;
        }

        //Report
        public async Task<IList<EMS_tblEmployeeExpense>> GetAllAsync_Rpt(DateTime startdate, DateTime endDate, string enrollNo)
        {
            MsSqlDataProvider objSql = new();
            List<int> employeeId = null;
            if (!string.IsNullOrEmpty(enrollNo))
            {
                var employee = await objSql.QueryAsync<tblEmployee>(@"select EmployeeID from tblEmployee Where EnrollNo IN (" + enrollNo.Trim() + ") ", null);
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
            }

            IQueryable<EMS_tblEmployeeExpense> query = from u in _EMS_tblEmployeeExpenseRepository.Table
                                                       select u;
            objSql = new();
            var data = await _EMS_tblEmployeeExpenseRepository.GetAllAsync(x=>x.Where(x => (employeeId == null || employeeId.Contains(x.EmployeeID)) && (x.ExpenseDate.Date >= startdate.Date && x.ExpenseDate.Date <= endDate.Date)));
            if (data != null && data.Count > 0)
            {
                var EmployeeIDList = data.Select(x => x.EmployeeID).ToList();
                if (EmployeeIDList != null && EmployeeIDList.Count > 0)
                {
                    var dataImageList = await objSql.QueryAsync<tblEmployeeDto>(@"select * from View_Employee Where EmployeeID IN (" + string.Join(",", EmployeeIDList) + ") ", null);
                    if (dataImageList != null && dataImageList.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            var imgObj = dataImageList.FirstOrDefault(c => c.EmployeeID == item.EmployeeID);
                            if (imgObj != null && imgObj.EmployeeID > 0)
                            {
                                item.EmployeeName = imgObj.EmployeeName;
                                item.DesignationName = imgObj.DesignationName;
                                item.DepartmentName = imgObj.DepartmentName;
                                item.DivisionName = imgObj.DivisionName;
                                item.EnrollNo = imgObj.EnrollNo;
                                item.AdharcardNo = imgObj.AdharcardNo;
                                item.SalaryType = imgObj.SalaryType;
                                item.BasicSalary = (imgObj.BasicSalary != null)?(decimal)imgObj.BasicSalary:null;
                            }
                        }
                    }
                }
                data = data.OrderBy(x => x.EnrollNo).ToList();
            }
            return data;

        }
        #endregion

        #endregion
    }
}