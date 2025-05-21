using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Dyeing;
using TheTecniQ.Core.Domain.DyeingProcess;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Services.Common;
using TheTecniQ.Services.Dyeing;
using TheTecniQ.Services.DyeingProcess;

namespace TheTecniQ.Api.Controllers.MobileAPI
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class PlanningwisePendingOrdersController(IPlanningwisePendingOrdersService planningwisePendingOrdersService, ISystemSettingService settingService) : BaseController
    {
        private readonly IPlanningwisePendingOrdersService _planningwisePendingOrdersService = planningwisePendingOrdersService;
        private readonly ISystemSettingService _settingService = settingService;


        [HttpPost]
        [Route("[action]")]
        //[Permission(Page = PageName.AdmAuditTrail, Permission = PagePermission.View)]
        public async Task<IActionResult> List(MobileGridRequestModel objGrid)
        {
            var lst = await _settingService.GetByKeyAsync("setting.mobilegridtotalrow");
            objGrid.PageSize = Convert.ToInt32((lst?.Value) ?? "20");            

            Core.IPagedList<PlanningwisePendingOrders> data = await _planningwisePendingOrdersService.GetAll(objGrid);
            return Ok(data.ToMobileListResponse(objGrid, "Data Logs"));
        }
        [HttpGet("{id?}")]        
        public async Task<ApiResponse> Get(string id)
        {
            IList<DyingPlanningView> objList = new List<DyingPlanningView>();
            extraDetail objExtra = new extraDetail();
            (objList, objExtra) = await _planningwisePendingOrdersService.GetDetail(id);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Record found.", objList, objExtra);
        }
    }
}
