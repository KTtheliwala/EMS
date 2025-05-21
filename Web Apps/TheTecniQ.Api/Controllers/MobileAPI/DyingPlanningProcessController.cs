using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.DyeingProcess;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Services.Common;
using TheTecniQ.Services.DyeingProcess;

namespace TheTecniQ.Api.Controllers.MobileAPI
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class DyingPlanningProcessController(IDyeingProductionPlanService dyeingProductionPlanService, ISystemSettingService settingService) : BaseController
    {
        private readonly IDyeingProductionPlanService _dyeingProductionPlanService = dyeingProductionPlanService;
        private readonly ISystemSettingService _settingService = settingService;

        [HttpPost]
        [Route("[action]")]
        //[Permission(Page = PageName.AdmAuditTrail, Permission = PagePermission.View)]
        public async Task<IActionResult> List(MobileGridRequestModel objGrid)
        {
            var lst = await _settingService.GetByKeyAsync("setting.mobilegridtotalrow");
            objGrid.PageSize = Convert.ToInt32((lst?.Value) ?? "20");            

            Core.IPagedList<MachineProgramGroup> data = await _dyeingProductionPlanService.GetAll(objGrid);
            return Ok(data.ToMobileListResponse(objGrid, "Data Logs"));
        }
    }
}
