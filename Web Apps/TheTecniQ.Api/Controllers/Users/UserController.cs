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
using TheTecniQ.Services.Users;
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

namespace TheTecniQ.Api.Controllers.Users
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class UserController(IUserService userService, IUserPasswordHistoryService userPasswordHistoryService, ISystemSettingService settingService) : BaseController
    {
        #region Fields
        private readonly IUserService _userService = userService;
        private readonly ISystemSettingService _settingService = settingService;
        private readonly IUserPasswordHistoryService _userPasswordHistoryService = userPasswordHistoryService;
        #endregion
        #region Messages
        private readonly string IsExistMsg = "User name already exist!";
        private readonly string SaveMsg = "User added successfully.";
        private readonly string UpdateMsg = "User updated successfully.";
        private readonly string DeleteMsg = "User deleted successfully.";
        private readonly string NotFoundMsg = "User not found.";
        #endregion

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmUser, Permission = PagePermission.View)]
        public async Task<IActionResult> List(GridRequestModel objGrid)
        {
            return Ok(await _userService.List(objGrid));
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.AdmUser, Permission = PagePermission.View)]
        public async Task<ApiResponse> Get(int id)
        {
            return await _userService.Get<EMS_User, UserModel>(id);
        }
        [HttpPost]
        [Permission(Page = PageName.AdmUser, Permission = PagePermission.AddOrEdit)]
        public async Task<ApiResponse> Post(UserModel model)
        {
            if (await _userService.IsNameExistAsync(query => { return query.Where(x => !x.IsDeleted && x.Id != model.Id && x.UserName.ToLower().TrimStart().TrimEnd() == model.UserName.ToLower().TrimStart().TrimEnd()); }))
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, IsExistMsg);
            }
            var ModelData = model.MapTo<EMS_User>();
            if (model.Id == 0)
            {
                if (!string.IsNullOrEmpty(model.Password))
                {
                    ModelData.Password = EncryptionUtility.CreatePasswordHash(model.Password);
                };
                ModelData.CreatedBy = CurrentUserId;
                ModelData.CreatedDate = DateTime.UtcNow;
                await _userService.InsertAsync(ModelData, CurrentUserId, CurrentUserName);
            }
            else
            {
                var existingUser = await _userService.GetByIdAsync(model.Id);
                if (existingUser != null)
                {
                    ModelData.CreatedBy = existingUser.CreatedBy;
                    ModelData.CreatedDate = existingUser.CreatedDate;
                }
                else
                {
                    return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status404NotFound, NotFoundMsg);
                }
                ModelData.Password = existingUser.Password;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    ModelData.Password = EncryptionUtility.CreatePasswordHash(model.Password);
                };
                ModelData.ModifyBy = CurrentUserId;
                ModelData.ModifyDate = DateTime.UtcNow;
                await _userService.UpdateAsync(ModelData, CurrentUserId, CurrentUserName);
            }
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, model.Id == 0 ? SaveMsg : UpdateMsg);
        }

        [HttpPatch]
        [Route("update-status")]
        [Permission(Page = PageName.AdmUser, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            return await _userService.UpdateStatus(id, CurrentUserId, CurrentUserName, UpdateMsg);
        }

        [HttpPatch]
        [Route("update-login-status")]
        [Permission(Page = PageName.AdmUser, Permission = PagePermission.Edit)]
        public async Task<ApiResponse> UpdateLoginStatus()
        {
            await _userService.GetCurrentLoginStatus(CurrentUserId);
            return APIResponseExtensions.GetResponse("Success", ApiStatusCode.Status200OK);
        }



        [HttpDelete]
        [Permission(Page = PageName.AdmUser, Permission = PagePermission.Delete)]
        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            return await _userService.Delete(Ids, CurrentUserId, CurrentUserName, DeleteMsg);
        }
        [HttpPatch]
        [Route("change-password")]
        [Permission]
        public async Task<ApiResponse> ChangePassword(ChangePasswordModel changePasswordmodel)
        {
            EMS_User PasswordUpdateObj = await _userService.GetByIdAsync(CurrentUserId);
            changePasswordmodel.Oldpassword = EncryptionUtility.CreatePasswordHash(changePasswordmodel.Oldpassword);
            if (PasswordUpdateObj.Password != changePasswordmodel.Oldpassword)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Invalid Old Password.");
            }

            PasswordUpdateObj.Password = EncryptionUtility.CreatePasswordHash(changePasswordmodel.Confirmpassword);
            int pwdpasswordhistory = 5;
            var passwordhistory =await _settingService.GetByKeyAsync("passwordhistory");
            if (!string.IsNullOrWhiteSpace(passwordhistory?.Value))
                pwdpasswordhistory = Convert.ToInt32(passwordhistory?.Value);
            var oldPassList = await _userPasswordHistoryService.GetLast5Async(CurrentUserId, pwdpasswordhistory);
            if (oldPassList != null)
            {
                var oldPassExisit = oldPassList.Where(x => x.Password == PasswordUpdateObj.Password).Count() > 0;
                if (oldPassExisit)
                {
                    return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "New password cannot be identical to the last " + passwordhistory.ToString() + " passwords.");
                }
            }

            PasswordUpdateObj.PasswordChangedOn = DateTime.UtcNow;
            await _userService.UpdateAsync(PasswordUpdateObj, CurrentUserId, CurrentUserName);

            await _userPasswordHistoryService.InsertAsync(new EMS_UserPasswordHistory
            {
                Password = PasswordUpdateObj.Password,
                UserId = CurrentUserId,
                CreatedOn = DateTime.UtcNow,
            }, CurrentUserId, CurrentUserName);

            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Password changed successfully.");
        }

        [HttpPost]
        [Route("update-profile")]
        [Permission]
        public async Task<ApiResponse> UpdateProfile(UserProfileModel userProfileModel)
        {
            EMS_User userProfileObj = await _userService.GetByIdAsync(CurrentUserId);
            userProfileObj.UserName = userProfileModel.UserName;
            userProfileObj.FirstName = userProfileModel.FirstName;
            userProfileObj.LastName = userProfileModel.LastName;
            userProfileObj.Email = userProfileModel.Email;
            userProfileObj.Mobile = userProfileModel.Mobile;
            await _userService.UpdateAsync(userProfileObj, CurrentUserId, CurrentUserName);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Profile updated successfully.");
        }

        [HttpGet]
        [Route("get-user-profile")]
        [Permission]
        public async Task<ApiResponse> GetUserProfile()
        {
            var data =  await _userService.GetByIdAsync(CurrentUserId);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "Profile updated successfully.", data);
        }
    }
}