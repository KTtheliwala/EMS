using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheTecniQ.API.Infrastructure;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Services.Common;
using TheTecniQ.Services.Employees;
using Asp.Versioning;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Users;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Api.Infrastructure;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Api.Models.Users;
using TheTecniQ.Core.Domain.Employees;
using TheTecniQ.API.Models.Masters;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Bibliography;

namespace TheTecniQ.Api.Controllers.Masters
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class EmployeeController(ICommonService<tblEmployee> emsUserService, IEmployeeService iEmployeeService, IEmployeeImagesService iEmployeeImagesService) : BaseController
    {
        #region Fields
        private readonly ICommonService<tblEmployee> _emsUserService = emsUserService;
        private readonly IEmployeeService _iEmployeeService = iEmployeeService;
        private readonly IEmployeeImagesService _iEmployeeImagesService = iEmployeeImagesService;
        #endregion
        #region Messages
        private readonly string IsEnrollExistMsg = "EnrollNo. already exist!";
        private readonly string SaveMsg = "Employee added successfully.";
        private readonly string UpdateMsg = "Employee updated successfully.";
        private readonly string DeleteMsg = "Employee deleted successfully.";
        private readonly string NotFoundMsg = "Employee not found.";
        #endregion

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.MstEmployee, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            Core.IPagedList<tblEmployeeDto> List = await _iEmployeeService.GetAllAsync(objGrid);
            return Ok(List.ToListResponse(objGrid, "Employee List", "EmployeeList"));
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.MstEmployee, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            var data = await _iEmployeeService.GetById(id);

            if (data == null)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status404NotFound, "Record not found.");
            }

            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Record found.", data);
        }

        [HttpPost]
        [Permission(Page = PageName.MstEmployee, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> Post([FromForm] EmployeeModel model)
        {
            if (!string.IsNullOrEmpty(model.StrDateofBirth))
                model.DateofBirth = Convert.ToDateTime(model.StrDateofBirth);

            if (await _iEmployeeService.CheckEnrollNo(model.EnrollNo.ToString(), model.EmployeeID))
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, IsEnrollExistMsg);
            }
            bool isUpdate = false;
            //Insert Employee
            if (model.EmployeeID == 0)
            {
                model.CreatedBy = CurrentUserId;
                model.CreatedDate = DateTime.UtcNow;
                model.EmployeeID = await _iEmployeeService.InsertAsync(model.MapTo<tblEmployee>());
            }
            else
            {                
                var ModelData = model.MapTo<tblEmployee>();
                tblEmployee obj = await _iEmployeeService.GetById(model.EmployeeID);
                ModelData.CreatedBy = obj.CreatedBy;
                ModelData.CreatedDate = obj.CreatedDate;
                await _iEmployeeService.UpdateAsync(ModelData);
                isUpdate = true;
            }
            if (model.EmployeeID > 0)
            {
                // Now manage files
                if ((model.ImageAadharCardFront != null && model.ImageAadharCardFront.Length > 0)
     || (model.ImageAadharCardBack != null && model.ImageAadharCardBack.Length > 0)
     || (model.ImageEmployeePhoto != null && model.ImageEmployeePhoto.Length > 0)
     || (model.ImageOtherDoc != null && model.ImageOtherDoc.Length > 0))
                {
                    // Helper method to set the image as byte array or return existing value
                    async Task<byte[]> GetImageByteArrayAsync(IFormFile file, byte[] existing)
                    {
                        return file != null && file.Length > 0 ? await SetImageAsByteArrayAsync(file) : existing;
                    }

                    tblEmployeeImage imgModel = new tblEmployeeImage
                    {
                        EmployeeID = model.EmployeeID,
                        ImageAadharCardFront = await GetImageByteArrayAsync(model.ImageAadharCardFront, null),
                        ImageAadharCardBack = await GetImageByteArrayAsync(model.ImageAadharCardBack, null),
                        ImageEmployeePhoto = await GetImageByteArrayAsync(model.ImageEmployeePhoto, null),
                        ImageOtherDoc = await GetImageByteArrayAsync(model.ImageOtherDoc, null)
                    };

                    tblEmployeeImage exist = await _iEmployeeImagesService.GetById(model.EmployeeID);

                    if (exist?.ImageNewID > 0)
                    {
                        // Set the values based on existing image or new image data
                        imgModel.EmployeeID = exist.EmployeeID;
                        imgModel.ImageAadharCardFront = await GetImageByteArrayAsync(model.ImageAadharCardFront, exist.ImageAadharCardFront);
                        imgModel.ImageAadharCardBack = await GetImageByteArrayAsync(model.ImageAadharCardBack, exist.ImageAadharCardBack);
                        imgModel.ImageEmployeePhoto = await GetImageByteArrayAsync(model.ImageEmployeePhoto, exist.ImageEmployeePhoto);
                        imgModel.ImageOtherDoc = await GetImageByteArrayAsync(model.ImageOtherDoc, exist.ImageOtherDoc);

                        await _iEmployeeImagesService.UpdateAsync(imgModel);  // Update image
                    }
                    else
                    {
                        // Insert new image
                        await _iEmployeeImagesService.InsertAsync(imgModel);
                        
                    }
                }
            }

            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, (!isUpdate) ? SaveMsg : UpdateMsg);
        }

        [NonAction]
        public async Task<byte[]> SetImageAsByteArrayAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream); // Asynchronously copy the content of IFormFile to the memory stream
                return memoryStream.ToArray(); // Convert the stream to byte array and return it
            }
            else
            {
                return null; // Return null if the file is null or empty
            }
        }


        [HttpPatch]
        [Route("update-status")]
        [Permission(Page = PageName.MstEmployee, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            if (id == 0)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "No data found.");
            }
            var data = await _iEmployeeService.GetById(id);
            data.IsActive = !data.IsActive;
            await _iEmployeeService.UpdateAsync(data);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, UpdateMsg);

        }

    }
}