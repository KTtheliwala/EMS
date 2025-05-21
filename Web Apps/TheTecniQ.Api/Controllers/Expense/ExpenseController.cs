using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheTecniQ.API.Infrastructure;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Services.Common;
using TheTecniQ.Services.Employees;
using Asp.Versioning;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Users;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Api.Infrastructure;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Api.Models.Users;
using TheTecniQ.Core.Domain.Employees;
using TheTecniQ.API.Models.Masters;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Bibliography;
using TheTecniQ.Core.Domain.Attendance;
using TheTecniQ.Services.Expense;
using TheTecniQ.API.Models.Expense;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TheTecniQ.Services.Attendance;

namespace TheTecniQ.Api.Controllers.Masters
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class ExpenseController(ICommonService<EMS_tblEmployeeExpense> emsEmployeeExpenseService, IEMS_tblEmployeeExpenseService eMS_TblEmployeeExpenseService,
        IAttendanceService employeeAttendanceService,
        ICommonService<EMS_tblEmployeeAttendance> ems_tblEmployeeAttendance) : BaseController
    {
        #region Fields
        private readonly ICommonService<EMS_tblEmployeeExpense> _emsEmployeeExpenseService = emsEmployeeExpenseService;
        private readonly ICommonService<EMS_tblEmployeeAttendance> _ems_tblEmployeeAttendance = ems_tblEmployeeAttendance;
        private readonly IEMS_tblEmployeeExpenseService _eMS_TblEmployeeExpenseService = eMS_TblEmployeeExpenseService;
        private readonly IAttendanceService _iemployeeAttendanceService = employeeAttendanceService;
        #endregion
        #region Messages
        private readonly string IsEnrollExistMsg = "EnrollNo. already exist!";
        private readonly string SaveMsg = "Expense added successfully.";
        private readonly string UpdateMsg = "Expense updated successfully.";
        private readonly string DeleteMsg = "Expense deleted successfully.";
        private readonly string NotFoundMsg = "Expense not found.";
        #endregion

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.MstExpense, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            if (objGrid.Filters != null)
            {
                var findDateFil = objGrid.Filters.Where(x => x.FieldName == "ExpenseDate").FirstOrDefault();
                if (!string.IsNullOrEmpty(findDateFil?.FieldValue))
                {
                    string[] dateParts = findDateFil.FieldValue.Split('-');
                    DateTime firstDate = DateTime.Parse(dateParts[0].Trim());

                    // Ensure firstDate is the 1st of the month
                    firstDate = new DateTime(firstDate.Year, firstDate.Month, 1);

                    // Find the last day of the month
                    DateTime secondDate = new DateTime(firstDate.Year, firstDate.Month, DateTime.DaysInMonth(firstDate.Year, firstDate.Month));

                    findDateFil.FieldValue = firstDate.ToString("dd MMM yyyy") + "-" + secondDate.ToString("dd MMM yyyy");

                }
            }
            Core.IPagedList<EMS_tblEmployeeExpense> List = await _eMS_TblEmployeeExpenseService.GetAllAsync(objGrid);
            return Ok(List.ToListResponse(objGrid, "Expense List", "ExpenseList"));
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.MstExpense, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            var data = await _emsEmployeeExpenseService.GetByIdAsync(id);

            if (data == null)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status404NotFound, "Record not found.");
            }

            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Record found.", data);
        }

        [HttpPost]
        [Permission(Page = PageName.MstExpense, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> Post(EMS_tblEmployeeExpenseModel model)
        {
            if (model == null || model.ExpenseDate == null || model.ExpenseDate.Date > DateTime.Now.Date)
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "The expense date must be today or a past date.");

            model.ExpenseNo = await _eMS_TblEmployeeExpenseService.GetMaxExpenseNo();

            model.ExpenseMonth = model.ExpenseDate.ToString("MMMM");
            var rtnData = await _emsEmployeeExpenseService.Post(model, null, CurrentUserId, CurrentUserName, "", SaveMsg, UpdateMsg);
            if (rtnData.StatusCode == 200)
            {
                var attendanceExist = await _iemployeeAttendanceService.GetEmployeeAttendence(model.EmployeeID, model.ExpenseDate);
                if (attendanceExist != null && attendanceExist?.Id > 0)
                {
                    //Get Total Expense
                    var getTotalExpense = await _iemployeeAttendanceService.GetEmployeeExpense(model.EmployeeID, model.ExpenseDate);
                    attendanceExist.TotalExpenseAmount = getTotalExpense;
                    attendanceExist.PayableAmount = (attendanceExist.TotalAmount - (getTotalExpense ?? 0));
                    await _ems_tblEmployeeAttendance.UpdateAsync(attendanceExist, CurrentUserId, CurrentUserName);
                }
            }

            return rtnData;
        }
        [HttpPost("check-activation")]
        [Permission(Page = PageName.MstExpense, Permission = PagePermission.View)]
        public async Task<ApiResponse> GetEmployeeActivation(EmployeeActivationCheck model)
        {

            if (model == null || model.EmployeeID == null || model.EmployeeID <= 0)
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Bad request");

            var data = await _eMS_TblEmployeeExpenseService.CheckEmployeeActivation(model.EmployeeID, model.ExpenseDate);
            var existingExpense = await _eMS_TblEmployeeExpenseService.getExistingExpense(model.EmployeeID, model.ExpenseDate);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "", new { Status = data, ExistingExpense = existingExpense });
        }

        [HttpDelete]
        [Permission(Page = PageName.MstExpense, Permission = PagePermission.Delete)]
        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            var expenseList = await _eMS_TblEmployeeExpenseService.getExpenseList(Ids);
            var rtnData = await _emsEmployeeExpenseService.Delete(Ids, CurrentUserId, CurrentUserName, DeleteMsg);
            if (rtnData.StatusCode == 200)
            {
                if (expenseList != null && expenseList.Count > 0)
                {
                    foreach (var item in expenseList.DistinctBy(x=>new { x.EmployeeID, x.ExpenseDate.Date.Year, x.ExpenseDate.Date.Month }))
                    {
                        var attendanceExist = await _iemployeeAttendanceService.GetEmployeeAttendence(item.EmployeeID, item.ExpenseDate);
                        if (attendanceExist != null && attendanceExist?.Id > 0)
                        {
                            //Get Total Expense
                            var getTotalExpense = await _iemployeeAttendanceService.GetEmployeeExpense(item.EmployeeID, item.ExpenseDate);
                            attendanceExist.TotalExpenseAmount = getTotalExpense;
                            attendanceExist.PayableAmount = (attendanceExist.TotalAmount - (getTotalExpense ?? 0));
                            await _ems_tblEmployeeAttendance.UpdateAsync(attendanceExist, CurrentUserId, CurrentUserName);
                        }
                    }
                }
            }
            return rtnData;
        }

    }
}