using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using Asp.Versioning;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Api.Infrastructure;
using System.Linq;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Services.Common;
using TheTecniQ.API.Models.Settings;

namespace TheTecniQ.Api.Controllers.Settings
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1")]
    public class SystemSettingController(ICommonService<Core.Domain.Settings.EMS_Settings> systemSettingService,
        ISystemSettingService settingService) : BaseController
    {
        #region Fields        
        private readonly ICommonService<Core.Domain.Settings.EMS_Settings> _ISystemSettingService = systemSettingService;
        private readonly ISystemSettingService _settingService = settingService;
        #endregion

        #region Messages
        private readonly string IsExistMsg = "Data is already exist!";
        private readonly string SaveMsg = "Data added successfully.";
        private readonly string UpdateMsg = "Data updated successfully.";
        private readonly string UpdateStatusMsg = "Status updated successfully.";
        #endregion

        #region System Setting
        /// <summary>
        /// From user side take number of row, page size, filter, sorting
        /// </summary>
        /// <param name="objGrid">Return the list of data based on user requested.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmSysSetting, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            return Ok(await _ISystemSettingService.List(objGrid));
        }
        /// <summary>
        /// This API used for return single record.
        /// </summary>
        /// <param name="id">pass unique key id from user side</param>
        /// <returns>Return object based on requested id.</returns>
        [HttpGet("{id?}")]
        [Permission(Page = PageName.AdmSysSetting, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            return await _ISystemSettingService.Get<Core.Domain.Settings.EMS_Settings, SettingModel>(id);
        }  
        /// <summary>
        /// This API used for return single record.
        /// </summary>
        /// <param name="id">pass unique key id from user side</param>
        /// <returns>Return object based on requested id.</returns>
        [HttpGet]
        [Permission(Page = PageName.AdmSysSetting, Permission = PagePermission.View)]
        public async Task<ApiResponse> GetByKey(string key)
        {
            if(string.IsNullOrEmpty(key))
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Bad request");
            var lst = await _settingService.GetByKeyAsync(key);
            if (lst == null)
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Key not exist");
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK,"Data list", lst);
        }
        [HttpPost("get-by-keys")]
        [Permission(Page = PageName.AdmSysSetting, Permission = PagePermission.View)]
        public async Task<ApiResponse> GetByKeys(string[] keys)
        {
            if (keys == null || keys.Length == 0)
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Bad request");
            var lst = await _settingService.GetByKeysAsync(keys);
            if (lst == null)
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Keys not exist");
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Data list", lst);

        }
        /// <summary>
        /// API used for Add/Update record
        /// </summary>
        /// <param name="model">Received object from user side and data Add/Updat in DB</param>
        /// <returns></returns>
        [HttpPost]
        [Permission(Page = PageName.AdmSysSetting, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> Post(SettingModel model)
        {
            return await _ISystemSettingService.Post(model, query =>
            {
                return query.Where(x => x.Id != model.Id && x.Key.ToLower().TrimStart().TrimEnd() == model.Key.ToLower().TrimStart().TrimEnd());
            }, CurrentUserId, CurrentUserName, IsExistMsg, SaveMsg, UpdateMsg);
        }
        /// <summary>
        /// Active/Inactive Status Update
        /// </summary>
        /// <param name="id">pass unique key id from user side</param>
        /// <returns>Return message & status code like 200 if successfully process done</returns>
        [HttpPatch]
        [Route("update-status")]
        [Permission(Page = PageName.AdmSysSetting, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            return await _ISystemSettingService.UpdateStatus(id, CurrentUserId, CurrentUserName, UpdateStatusMsg);
        }

        #endregion
    }
}
