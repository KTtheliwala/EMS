using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using Asp.Versioning;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Models.Users;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Api.Infrastructure;
using System.Linq;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Services.Common;
using TheTecniQ.Core.Domain.Attendance;
using TheTecniQ.Services.Attendance;
using TheTecniQ.API.Models.Attendance;
using TheTecniQ.Core.Domain.Employees;
using System;
using TheTecniQ.API.Models.Expense;
using SharpCompress;

namespace TheTecniQ.Api.Controllers.Users
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class EmployeeAttendanceController(ICommonService<EMS_tblEmployeeAttendance> tblEmployeeAttendanceService, IAttendanceService employeeAttendanceService) : BaseController
    {
        #region Fields
        private readonly ICommonService<EMS_tblEmployeeAttendance> _tblEmployeeAttendanceService = tblEmployeeAttendanceService;
        private readonly IAttendanceService _iemployeeAttendanceService = employeeAttendanceService;
        #endregion

        #region Messages
        private readonly string IsExistMsg = "Attendance is already exist for selected employee!";
        private readonly string SaveMsg = "Attendance added successfully.";
        private readonly string UpdateMsg = "Attendance updated successfully.";
        private readonly string DeleteMsg = "Attendance deleted successfully.";
        #endregion

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.MstAttendance, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            if (objGrid.Filters != null)
            {
                var findDateFil = objGrid.Filters.Where(x => x.FieldName == "AttendanceDate").FirstOrDefault();
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
            Core.IPagedList<EMS_tblEmployeeAttendance> List = await _iemployeeAttendanceService.GetAllAsync(objGrid);
            return Ok(List.ToListResponse(objGrid, "Activation List", "ActivationList"));            
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.MstAttendance, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            var data = await _tblEmployeeAttendanceService.GetByIdAsync(id);

            if (data == null)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status404NotFound, "Record not found.");
            }
            else {
                var getExpense = await _iemployeeAttendanceService.GetEmployeeExpense(data.EmployeeID, data.AttendanceDate);
                if (getExpense != null && getExpense > 0)
                {
                    if (data.TotalExpenseAmount != getExpense)
                    {
                        data.rowClass = "alert-mismatch";
                    }
                }
            }

            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Record found.", data);
        }

        [HttpPost]
        [Permission(Page = PageName.MstAttendance, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> Post(EMS_tblEmployeeAttendanceModel model)
        {
            return await _tblEmployeeAttendanceService.Post(model, query =>
            {
                return query.Where(x => x.Id != model.Id && x.EmployeeID == model.EmployeeID && x.AttendanceDate.Date == model.AttendanceDate.Date);
            }, CurrentUserId, CurrentUserName, IsExistMsg, SaveMsg, UpdateMsg);
        }

        [HttpPost("get-employee-expense")]
        [Permission(Page = PageName.MstAttendance, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> GetEmployeeExpense(EmployeeActivationCheck model)
        {

            if (model == null ||((model.EmployeeID == null || model.EmployeeID <= 0) && (model.ExpenseDate == null || model.ExpenseDate.Date > DateTime.Now.Date)))
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Bad request");

            var data = await _iemployeeAttendanceService.GetEmployeeExpense(model.EmployeeID, model.ExpenseDate);

            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "", new { Status = data??0 });
        }

        [HttpPost("active-employee-list")]
        [Permission(Page = PageName.MstAttendance, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> ActiveEmployeeList(SearchActiveEmployeeModel model)
        {
            var data = await _iemployeeAttendanceService.ActiveEmployeeSearch(model);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "", data);
        }

        [HttpDelete]
        [Permission(Page = PageName.MstAttendance, Permission = PagePermission.Delete)]
        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            return await _tblEmployeeAttendanceService.Delete(Ids, CurrentUserId, CurrentUserName, DeleteMsg);
        }
    }
}
