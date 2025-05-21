using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Services.Common;
using Asp.Versioning;
using DocumentFormat.OpenXml.Office2016.ExcelAc;
using Microsoft.AspNetCore.Authentication.Cookies;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Data.Extensions;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Services.Users;
using TheTecniQ.Services.Logging;
using TheTecniQ.Services.Permission;
using TheTecniQ.Services.Masters;
using TheTecniQ.Services.Reading;

namespace TheTecniQ.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]

    public class DropDownController(
        ICommonService<EMS_Role> _roleService,
        IPermissionService _permissionService,
        ILogService logService,
        IDepartmentService departmentService,
        IDivisionService divisionService,
        IDesignationService designationService,
        ItblDGVCLDBUnitsService itblDGVCLDBUnitsService,
        IUserService _userService) : BaseController
    {

        private readonly ICommonService<EMS_Role> _IRoleService = _roleService;
        private readonly IPermissionService _IPermissionService = _permissionService;
        IUserService _IUserService = _userService;
        ILogService _LogService = logService;
        IDepartmentService _departmentService = departmentService;
        IDivisionService _divisionService = divisionService;
        IDesignationService _designationService = designationService;
        ItblDGVCLDBUnitsService _tblDGVCLDBUnitsService = itblDGVCLDBUnitsService;
        #region  :: Drop Down Get Data ::
        [HttpGet]
        [Route("[action]")]
        [Permission]
        public async Task<ActionResult<ListResponse>> GetBindDropDown(string mode, int? Id)
        {
            IList<SelectListItem> Result = mode switch
            {
                "Role" => (await _IRoleService.GetAllAsync()).ToDropDown(),
                "Department" => (await _departmentService.GetAll()).ToDropDown("DepartmentID", "DepartmentName"),
                "Division" => (await _divisionService.GetAll()).ToDropDown("DivisionID", "DivisionName"),
                "Designation" => (await _designationService.GetAll(Id)).ToDropDown("DesignationID", "DesignationName"),
                "Panel" => (await _tblDGVCLDBUnitsService.getTblDBPanelMaster()).ToDropDown("DBPanelID", "DBName"),
                "DB" => (await _tblDGVCLDBUnitsService.getTblDBUnits()).ToDropDown("DBID", "DBName"),
                "AllUser" => (await _IUserService.GetDataForDropdown(null)).ToDropDown("Id", "FullName"),
                "LogSource" => typeof(EnumLogSource).ToSelectListItems(),
                "LogType" => typeof(EnumLogType).ToSelectListItems(),
                "LogEntity" => _LogService.GetddlData().ToDropDown("Entity", "Entity"),
                "PageName" => (await _IPermissionService.GetPageDataForDropdown()).ToDropDown("PageName", "TableNames"),
                "LogAction" => typeof(EnumLogAction).ToSelectListItems(),                
                "DispatchStatus" => CommonExtensions.ToSelectListItemsDescription(typeof(EnumDispatchQtyStatus), true),
                "ImportDetailStatus" => CommonExtensions.ToSelectListItemsDescription(typeof(EnumImportDetailStatus), true),
                "RechargeStaus" => CommonExtensions.ToSelectListItemsDescription(typeof(RechargeStaus),true),
                "RechargeStausWithCF" => CommonExtensions.ToSelectListItemsDescription(typeof(RechargeStausWithCarryForward), true),
                _ => []
            };
            return Result.Select(x => new { x.Value, x.Text }).ToResponse("Get Data Successfully.");
        }
        #endregion

    }
}
