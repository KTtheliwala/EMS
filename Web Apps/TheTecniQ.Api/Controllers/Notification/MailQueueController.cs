using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Grid;
using Asp.Versioning;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Api.Infrastructure;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Services.Common;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.API.Models.Notification;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Services.Notification;

namespace TheTecniQ.Api.Controllers.Notification
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class MailQueueController(ICommonService<EMS_MailQueue> ColorService, IMailQueueService mailQueueService) : BaseController
    {
        #region Fields
        private readonly ICommonService<EMS_MailQueue> _IMailQueueService = ColorService;
        private readonly IMailQueueService _mailQueueService = mailQueueService;
        #endregion

        #region Messages
        #endregion

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmMailQueue, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            Core.IPagedList<EMS_MailQueue> List = await _mailQueueService.GetAllAsync(objGrid);
            return Ok(List.ToListResponse(objGrid, "List"));
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.AdmMailQueue, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            return await _IMailQueueService.Get<EMS_MailQueue, MailQueueModel>(id);
        }

        [HttpPatch]
        [Route("[action]")]
        [Permission(Page = PageName.AdmMailQueue, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> RetryEmailSync([FromBody] int id)
        {
            if (id > 0)
            {
                EMS_MailQueue data = await _mailQueueService.GetByIdAsync(id);
                data.Status = 0;
                data.Retry = data.Retry > 0 ? data.Retry - 1 : data.Retry;
                await _mailQueueService.UpdateAsync(data, CurrentUserId, CurrentUser.ToString());
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Mail Queue updated successfully.", data);
            }
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "No data found.");
        }
    }
}
