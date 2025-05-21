using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Models.Settings;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Services.Common;
using TheTecniQ.Services.Notification;
using System.Threading.Tasks;
using System.Linq;
using TheTecniQ.Api.Infrastructure;
using TheTecniQ.API.Models.Notification;


namespace TheTecniQ.Api.Controllers.Notification
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1")]
    public class EmailTemplateController(ICommonService<EMS_EmailTemplate> emailTemplateService) : BaseController
    {
        #region Fields        
        private readonly ICommonService<EMS_EmailTemplate> _IEmailTemplateService = emailTemplateService;
        #endregion

        #region Messages
        private readonly string IsExistMsg = "Email template code is already exist!";
        private readonly string SaveMsg = "Email template added successfully.";
        private readonly string UpdateMsg = "Email template updated successfully.";
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
        [Permission(Page = PageName.AdmEmailTemplate, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            return Ok(await _IEmailTemplateService.List(objGrid));
        }
        /// <summary>
        /// This API used for return single record.
        /// </summary>
        /// <param name="id">pass unique key id from user side</param>
        /// <returns>Return object based on requested id.</returns>
        [HttpGet("{id?}")]
        [Permission(Page = PageName.AdmEmailTemplate, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            return await _IEmailTemplateService.Get<EMS_EmailTemplate, EmailTemplateModel>(id);
        }
        /// <summary>
        /// API used for Add/Update record
        /// </summary>
        /// <param name="model">Received object from user side and data Add/Updat in DB</param>
        /// <returns></returns>
        [HttpPost]
        [Permission(Page = PageName.AdmEmailTemplate, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> Post(EmailTemplateModel model)
        {
            return await _IEmailTemplateService.Post(model, query =>
            {
                return query.Where(x => x.Id != model.Id && x.TemplateCode.ToLower().TrimStart().TrimEnd() == model.TemplateCode.ToLower().TrimStart().TrimEnd());
            }, CurrentUserId, CurrentUserName, IsExistMsg, SaveMsg, UpdateMsg);
        }
        /// <summary>
        /// Active/Inactive Status Update
        /// </summary>
        /// <param name="id">pass unique key id from user side</param>
        /// <returns>Return message & status code like 200 if successfully process done</returns>
        [HttpPatch]
        [Route("update-status")]
        [Permission(Page = PageName.AdmEmailTemplate, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            return await _IEmailTemplateService.UpdateStatus(id, CurrentUserId, CurrentUserName, UpdateStatusMsg);
        }

        #endregion
    }
}
