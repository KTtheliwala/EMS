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

namespace TheTecniQ.Api.Controllers.Users
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class RoleController(ICommonService<EMS_Role> roleService) : BaseController
    {
        #region Fields
        private readonly ICommonService<EMS_Role> _roleService = roleService;
        #endregion

        #region Messages
        private readonly string IsExistMsg = "This Role is already exist!";
        private readonly string SaveMsg = "Role added successfully.";
        private readonly string UpdateMsg = "Role updated successfully.";
        private readonly string DeleteMsg = "Role deleted successfully.";
        #endregion

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmRole, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            return Ok(await _roleService.List(objGrid));
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.AdmRole, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            return await _roleService.Get<EMS_Role, RoleModel>(id);
        }

        [HttpPost]
        [Permission(Page = PageName.AdmRole, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> Post(RoleModel model)
        {
            return await _roleService.Post(model, query =>
            {
                return query.Where(x => x.Id != model.Id && x.Name.ToLower().TrimStart().TrimEnd() == model.Name.ToLower().TrimStart().TrimEnd());
            }, CurrentUserId, CurrentUserName, IsExistMsg, SaveMsg, UpdateMsg);
        }

        [HttpPatch]
        [Route("update-status")]
        [Permission(Page = PageName.AdmRole, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            return await _roleService.UpdateStatus(id, CurrentUserId, CurrentUserName, UpdateMsg);
        }

        [HttpDelete]
        [Permission(Page = PageName.AdmRole, Permission = PagePermission.Delete)]
        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            return await _roleService.Delete(Ids, CurrentUserId, CurrentUserName, DeleteMsg);
        }
    }
}
