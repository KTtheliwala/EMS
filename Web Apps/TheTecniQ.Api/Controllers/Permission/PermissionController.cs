using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using Asp.Versioning;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Models.Masters;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Api.Infrastructure;
using System.Linq;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Services.Common;
using System;
using TheTecniQ.Core.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using TheTecniQ.Services.Users;

using DocumentFormat.OpenXml.EMMA;
using Azure;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using TheTecniQ.Services.Permission;

namespace TheTecniQ.Api.Controllers.Permission
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class PermissionController(IPermissionService iPermissionService) : BaseController
    {
        #region Fields
        private readonly IPermissionService _IPermissionService = iPermissionService;

        #endregion
        #region Ctor

        #endregion


        [HttpPost]
        [Route("[action]")]
        [Permission]
        public async Task<ApiResponse> SavePermission(List<Core.Domain.Permissions.EMS_Permission> data)
        {
            await _IPermissionService.InsertAsync(data, CurrentUserId, CurrentUserName);
            return "Permission updated successfully.".GetResponse(ApiStatusCode.Status200OK);
        }

        [HttpGet]
        [Route("get-role-based-permission-data")]
        [Permission(Page = PageName.AdmRolePermission, Permission = PagePermission.View)]
        public async Task<ApiResponse> RoleBasedPermissionData(int id)
        {
            IList<PagePermisson> PermissionList = await _IPermissionService.GetRoleBasedPermissionData(id);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Get Data Successfully", new { PermissionList });
        }
    }
}
