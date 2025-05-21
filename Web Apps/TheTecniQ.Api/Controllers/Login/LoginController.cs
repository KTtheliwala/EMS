using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Models.Users;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Services.Logging;
using TheTecniQ.Services.Permission;
using TheTecniQ.Services.Users;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System;
using TheTecniQ.Services.Common;
using TheTecniQ.Api.Models.Login;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Settings;
using TheTecniQ.Core.Domain.Logging;
using Google.Authenticator;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Core.Configuration;
using System.Linq;
using Serilog.Context;

namespace TheTecniQ.Api.Controllers.Login
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class LoginController(IUserService UserService, ISystemSettingService settingService,
             IPermissionService IPermissionRepository, ILogService logService) : BaseController
    {
        #region Fields
        private readonly IUserService _UserService = UserService;
        private readonly ISystemSettingService _settingService = settingService;
        private readonly IPermissionService _IPermissionRepository = IPermissionRepository;
        private readonly ILogService _logService = logService;

        #endregion

        [HttpPost]
        [Route("[action]")]
        [SwaggerOperation(Description = "for get CaptchaCode & CaptchaToken call GetCaptcha (Next) API.")]
        public async Task<ApiResponse> Authenticate([FromBody] AuthenticateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CaptchaCode) && model.Platform == PlatForm.Desktop.ToDescription())
            {
                return APIResponseExtensions.GetResponse("Captcha code is required", ApiStatusCode.Status400BadRequest);
            }
            else if (string.IsNullOrWhiteSpace(model.Username))
            {
                return APIResponseExtensions.GetResponse("Username is required", ApiStatusCode.Status400BadRequest);
            }
            else if (string.IsNullOrWhiteSpace(model.Password))
            {
                return APIResponseExtensions.GetResponse("Password is required", ApiStatusCode.Status400BadRequest);
            }
            else
            {
                if (VerifyCaptcha(model.CaptchaCode, model.CaptchaToken, model.Platform) || model.Password.Trim().Length == 0)
                {
                    #region :: AD Login ::
                    EMS_User user = await _UserService.CheckLoginAsync(model.Username, model.Password);
                    if (user == null)
                    {
                       // await _logService.AuditAsync(EnumLogAction.Login, 0, "", 0, "Login", "Login failed with username=" + model.Username, null, EnumLogType.Error);
                        return APIResponseExtensions.GetResponse("Authentication Failed! Invalid username or password.", ApiStatusCode.Status400BadRequest);
                    }
                    else
                    {
                        //if ((user.IsLoginActive ?? false) && model.Confirm == false)
                        //{
                        //    return APIResponseExtensions.GetResponse("Are you sure confirm to login. Currenltly this user logged in.", ApiStatusCode.Status512AlreadyLogin);
                        //}
                        //else 
                        //{
                           // await _iResultService.AdjustAutoResultInsertAsync(user.Id, user.UserName);
                            //await branchSheetService.UpdateCurrentBalanceLoginTime();
                            return await AfterLoginProcess(user);
                             
                        //}
                    }
                    #endregion
                }
                else
                {
                    return APIResponseExtensions.GetResponse("Captcha code is expired OR Invalid", ApiStatusCode.Status400BadRequest);
                }
            }
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<ApiResponse> LoginWith2FA([FromBody] AuthenticateWith2FAModel model)
        {
            TwoFactorAuthenticator twoFactor = new();
            int UserId = EncryptionUtility.Decrypt(model.UserId, AppConfig.AESKeys.Key, AppConfig.AESKeys.IV).ToInt();
            if (UserId <= 0)
                return APIResponseExtensions.GetResponse("Invalid userid.", ApiStatusCode.Status400BadRequest);
            var user = await _UserService.GetByIdAsync(UserId);
            if (user == null || user.Id == 0)
                return APIResponseExtensions.GetResponse("Invalid userid.", ApiStatusCode.Status400BadRequest);
            //user.TwoFaSecretKey = string.IsNullOrWhiteSpace(user.TwoFaSecretKey) ? AppConfig.TwoFactor.SecretKeyFormat.Replace("{{UserId}}", user.Id.ToString()) : user.TwoFaSecretKey;
            bool isValid = false;//twoFactor.ValidateTwoFactorPIN(user.TwoFaSecretKey, model.Code);
            if (isValid)
            {
                return await AfterLoginProcess(user);
            }
            else
            {
                return APIResponseExtensions.GetResponse("Invalid captcha.", ApiStatusCode.Status400BadRequest);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ApiResponse> Logout()
        {
            EMS_User user = await _UserService.GetByIdAsync(CurrentUserId);
            if (user != null)
            {

                user.UserToken = Guid.NewGuid().ToString();
                await _UserService.UpdateAsync(user, CurrentUserId, CurrentUserName);
                return APIResponseExtensions.GetResponse("Success", ApiStatusCode.Status200OK);
            }
            return APIResponseExtensions.GetResponse("Error", ApiStatusCode.Status400BadRequest);
        }

        [HttpPost("RefreshToken")]
        public async Task<ApiResponse> RefreshToken([FromBody] RefreshAuthenticateModel model)
        {
            IPrincipal userClaim = HttpContext.User;
            if ((userClaim.Identity as ClaimsIdentity).Claims.Any())
            {
                if (userClaim.Identity.IsAuthenticated)
                {
                    if (DateTime.Compare(CommonExtensions.FromUnixTime(long.Parse((userClaim.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "exp").Value)), DateTime.UtcNow) > 0)
                    {
                        string oldToken = CommonExtensions.GetTokenValue(userClaim, "UserToken");
                        //if (oldToken == model.UserToken || string.IsNullOrEmpty(oldToken))
                        {
                            int UserId = Convert.ToInt32(CommonExtensions.GetTokenValue(userClaim, "Id"));
                            EMS_User user = await _UserService.GetByIdAsync(UserId);

                            if (user != null)
                            {
                                //if (user.UserToken == model.UserToken)
                                {
                                    return await AfterLoginProcess(user);
                                }
                            }
                        }
                    }
                }
            }

            return APIResponseExtensions.GetResponse("Authentication Failed! Invalid Token.", ApiStatusCode.Status401Unauthorized);
        }


        [HttpGet]
        [Route("[action]")]
        [SwaggerOperation(Description = "for see actual captcha open <a target='_blank' href='https://codebeautify.org/base64-to-image-converter'>Link</a> and then paste string in textbox, so you can see actual captcha.")]
        public ApiResponse GetCaptcha()
        {
            const int width = 200;
            const int height = 60;
            string captchaCode = Captcha.GenerateCaptchaCode();
            CaptchaResult result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            string encrToken = EncryptionUtility.Encrypt(result.CaptchaCode + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), AppConfig.AESKeys.Key, AppConfig.AESKeys.IV);
            return new ApiResponse() { Data = new { Img = result.CaptchBase64Data, Token = encrToken }, StatusCode = 200, StatusText = "OK" };
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ApiResponse> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CaptchaCode))
            {
                return APIResponseExtensions.GetResponse("Captcha code is required", ApiStatusCode.Status400BadRequest);
            }

            if (VerifyCaptcha(model.CaptchaCode, model.CaptchaToken, model.Platform))
            {
                if (model.Email != null)
                {
                    EMS_User user = await _UserService.GetUserByUserName(model.Email); //User user = await _UserService.GetByEmailAsync(model.Email);
                    if (user.Id != 0)
                    {
                         //await _UserService.ResetTokenAsync(user, CurrentUserId, CurrentUserName, SecurityKeys.EnDeKey);
                        return APIResponseExtensions.GetResponse("Password reset link sent to your email address", ApiStatusCode.Status200OK);
                    }
                    return APIResponseExtensions.GetResponse("User not found", ApiStatusCode.Status400BadRequest);
                }
            }
            else
            {
                return APIResponseExtensions.GetResponse("Captcha code is expired OR Invalid", ApiStatusCode.Status400BadRequest);
            }
            return APIResponseExtensions.GetResponse("", ApiStatusCode.Status400BadRequest);
        }


        [NonAction]
        public async Task<ApiResponse> AfterLoginProcess(EMS_User user)
        {
            user.UserToken = Guid.NewGuid().ToString();
            user.LastLoggedInOn = DateTime.UtcNow;
            user.ModifyBy = user.Id;
            user.ModifyDate = DateTime.UtcNow;
            await _UserService.UpdateAsync(user, user.Id, user.UserName);
            (var permissionString, var permissions) = await GetPermissions(user.RoleId);
            var lst = await _settingService.GetByKeyAsync("setting.apitokenexpiry");
            //await _logService.AuditAsync(EnumLogAction.Login, user.Id, user.UserName, user.Id, "Login", "Login Successfully.");
            return APIResponseExtensions.GetResponse("Login Successfully.", ApiStatusCode.Status200OK, new
            {
                //user.UserToken,
                Token = CommonExtensions.GenerateToken(user,null, AppConfig.Authentication.SecretKey, Convert.ToInt16((lst?.Value) ?? "0"), permissionString, AppConfig.Authentication.Issuer, AppConfig.Authentication.Audience),
                Permissions = EncryptionUtility.Encrypt(JsonConvert.SerializeObject(permissions), AppConfig.AESKeys.Key, AppConfig.AESKeys.IV),
                UserName = user.FirstName + " " + user.LastName,
                UserId = user.Id
            });
        }
        [NonAction]
        public async Task<(string menuHideString, List<EMS_Page> permissions)> GetPermissions(int RoleId)
        {
            List<EMS_Page> permissions = await _IPermissionRepository.GetAllModules(RoleId);
            StringBuilder permissionString = new();
            foreach (EMS_Page objPage in permissions)
            {
                if (RoleId == 1)
                {
                    objPage.IsAdd = true;
                    objPage.IsEdit = true;
                    objPage.IsDelete = true;
                    objPage.IsView = true;
                }
                permissionString.Append(objPage.PageCode).Append('|').Append(objPage.IsAdd ? 1 : 0).Append('|').Append(objPage.IsEdit ? 1 : 0).Append('|').Append(objPage.IsDelete ? 1 : 0).Append('|').Append(objPage.IsView ? 1 : 0).Append('|').Append(',');

            }
            return (permissionString.ToString(), permissions);
        }
        [NonAction]
        public bool VerifyCaptcha(string CaptchaCode, string CaptchaToken, string platform)
        {
            if (platform == PlatForm.Mobile.ToDescription())
                return true;
            if (string.IsNullOrWhiteSpace(CaptchaCode) || string.IsNullOrWhiteSpace(CaptchaToken))
            {
                return false;
            }
            string[] decrypt = EncryptionUtility.Decrypt(CaptchaToken, AppConfig.AESKeys.Key, AppConfig.AESKeys.IV).Split('|');
            return CaptchaCode == decrypt[0] && Convert.ToDateTime(decrypt[1]).AddMinutes(2) >= DateTime.Now;
        }


    }
}
