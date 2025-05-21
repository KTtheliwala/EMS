using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Api.Models.Reading;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Models.Users;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Core.Domain.Reading;
using TheTecniQ.Services.Reading;

namespace TheTecniQ.Api.Controllers.MobileAPI
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class ReadingController(ItblDGVCLDBUnitsService itblDGVCLDBUnitsService) : BaseController
    {
        public readonly ItblDGVCLDBUnitsService _itblDGVCLDBUnitsService = itblDGVCLDBUnitsService;
        

        #region Messages
        private readonly string IsExistMsg = "Reading already exist!";
        private readonly string SaveMsg = "Reading added successfully.";
        private readonly string UpdateMsg = "User updated successfully.";
        private readonly string DeleteMsg = "User deleted successfully.";
        private readonly string NotFoundMsg = "User not found.";
        #endregion

        [HttpPost("create-panel-reading")]
        [Permission]
        public async Task<ApiResponse> CreatePanelReading(tblDGVCLDBUnitsModel model)
        {
            var ModelData = model.MapTo<tblDGVCLDBUnits>();
            if (await _itblDGVCLDBUnitsService.CheckAlreadyExist(ModelData))
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, IsExistMsg);
            }
            if (model.DBDGVCLUnitID == 0)
            {
                ModelData.CRBy = CurrentUserId;
                ModelData.CRDate = DateTime.Now;
                await _itblDGVCLDBUnitsService.InsertAsync(ModelData);
            }
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, SaveMsg);
        }

        [HttpPost("create-db-reading")]
        [Permission]
        public async Task<ApiResponse> CreateDBReading(tblDBUnitsModel model)
        {
            var ModelData = model.MapTo<tblDBUnits>();
            if (await _itblDGVCLDBUnitsService.CheckAlreadyExist(ModelData))
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, IsExistMsg);
            }
            if (model.DBUnitID == 0)
            {                
                await _itblDGVCLDBUnitsService.InsertAsync(ModelData);
            }
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, SaveMsg);
        }

        [HttpGet("get-panel-reading")]
        [Permission]
        public async Task<ApiResponse> GetPanelReading(string dt)
        {
            DateTime parsedDt = DateTime.Now;
            if (string.IsNullOrWhiteSpace(dt))
                parsedDt = DateTime.Now;
            else
            if (!DateTime.TryParseExact(dt, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDt))
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Invalid date format. Expected format: yyyy-MM-dd");
            }

            ReadingDto objData = new ReadingDto();
            objData.panelReading = await _itblDGVCLDBUnitsService.ListTblDGVCLDBUnits(parsedDt);
            objData.dbReading = null;
            
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "List of data", objData);
        }
        [HttpGet("get-db-reading")]
        [Permission]
        public async Task<ApiResponse> GetDbReading(string dt)
        {
            DateTime parsedDt = DateTime.Now;
            if (string.IsNullOrWhiteSpace(dt))
                parsedDt = DateTime.Now;
            else
            if (!DateTime.TryParseExact(dt, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDt))
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Invalid date format. Expected format: yyyy-MM-dd");
            }

            ReadingDto objData = new ReadingDto();
            objData.panelReading = null;
            objData.dbReading = await _itblDGVCLDBUnitsService.ListTblDBUnits(parsedDt);

            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "List of data", objData);
        }

    }
}
