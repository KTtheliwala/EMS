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

namespace TheTecniQ.Api.Controllers.Users
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class AttendanceActivationController(ICommonService<EMS_tblAttendanceActivation> tblAttendanceService, IActivationService activationService) : BaseController
    {
        #region Fields
        private readonly ICommonService<EMS_tblAttendanceActivation> _tblAttendanceService = tblAttendanceService;
        private readonly IActivationService _iActivationService = activationService;
        #endregion

        #region Messages
        private readonly string IsExistMsg = "Activation is already exist for selected employee!";
        private readonly string SaveMsg = "Activation added successfully.";
        private readonly string UpdateMsg = "Activation updated successfully.";
        private readonly string DeleteMsg = "Activation deleted successfully.";
        #endregion

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.MstActivation, Permission = PagePermission.View)]
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
            Core.IPagedList<EMS_tblAttendanceActivation> List = await _iActivationService.GetAllAsync(objGrid);
            return Ok(List.ToListResponse(objGrid, "Activation List", "ActivationList"));            
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.MstActivation, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            return await _tblAttendanceService.Get<EMS_tblAttendanceActivation, AttendanceActivationModel>(id);
        }

        [HttpPost]
        [Permission(Page = PageName.MstActivation, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> Post(AttendanceActivationModel model)
        {
            return await _tblAttendanceService.Post(model, query =>
            {
                return query.Where(x => x.Id != model.Id && x.EmployeeID == model.EmployeeID && x.AttendanceDate.Date == model.AttendanceDate.Date);
            }, CurrentUserId, CurrentUserName, IsExistMsg, SaveMsg, UpdateMsg);
        }

        [HttpPost("search-employee")]
        [Permission(Page = PageName.MstActivation, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> SearchEmployee(SearchEmployeeModel model)
        {
            var data = await _iActivationService.EmployeeSearch(model);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK,"", data);            
        }

        
        [HttpPatch]
        [Route("update-status")]
        [Permission(Page = PageName.MstActivation, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            return await _tblAttendanceService.UpdateStatus(id, CurrentUserId, CurrentUserName, UpdateMsg);
        }

        [HttpDelete]
        [Permission(Page = PageName.MstActivation, Permission = PagePermission.Delete)]
        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            return await _tblAttendanceService.Delete(Ids, CurrentUserId, CurrentUserName, DeleteMsg);
        }
    }
}
