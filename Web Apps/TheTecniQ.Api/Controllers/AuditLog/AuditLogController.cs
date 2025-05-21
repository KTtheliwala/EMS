using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Models;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Services.Logging;
using System.Threading.Tasks;
using TheTecniQ.API.Models.Masters;

using TheTecniQ.Api.Models.AuditLog;

namespace TheTecniQ.Api.Controllers.AuditLog
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class AuditLogController(ILogService logService) : ControllerBase
    {
        #region Fields
        private readonly ILogService _logService = logService;

        #endregion
        #region Ctor
        #endregion


        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmAuditTrail, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            Core.IPagedList<Core.Domain.Logging.Logs> auditlog = await _logService.GetAll(objGrid);
            return Ok(auditlog.ToListResponse(objGrid, "Audit Logs"));
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.AdmAuditTrail, Permission = PagePermission.View)]
        public async Task<ActionResult> Get(int id)
        {
            Core.Domain.Logging.Logs data = await _logService.GetLogByIdAsync(id);
            return data.ToResponse("Get Data");
        }
    }
}
