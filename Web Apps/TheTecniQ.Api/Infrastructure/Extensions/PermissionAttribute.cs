using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Services.Users;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System;
using System.Linq;
using TheTecniQ.Core.Configuration;

namespace TheTecniQ.Api.Infrastructure.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        public string Page { get; set; }
        public TokenRole Role { get; set; }
        public PagePermission Permission { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorised = false;
            bool isPermission = true;
            string tokenRole = Role == TokenRole.Branch ? "Branch" : "Admin";
            IPrincipal user = context.HttpContext.User;
            UserService IUserMasterRepository = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as UserService;
            if ((user.Identity as ClaimsIdentity).Claims.Any())
            {
                string UserToken = GetTokenValue(user, "UserToken");
                string UserId = GetTokenValue(user, "Id");
                if (tokenRole == "Admin")
                {                   
                    if (IUserMasterRepository.CheckTokenIsValidAsync(UserId.ToInt(), UserToken) && user.Identity.IsAuthenticated)
                    {
                        isAuthorised = true;
                        if (DateTime.Compare(CommonExtensions.FromUnixTime(long.Parse((user.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "exp").Value)), DateTime.UtcNow) < 0)
                        {
                            isAuthorised = false;
                        }
                        else if ((Page?.Trim() ?? "") != "")
                        {
                            isPermission = CommonExtensions.CheckPermission(Page, Permission);
                        }
                    }
                }                
            }
            if (!isAuthorised)
            {
                string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                if (!actionName.Equals("REFRESHTOKEN", StringComparison.CurrentCultureIgnoreCase))
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                    context.Result = new JsonResult("NotAuthorized")
                    {
                        Value = new
                        {
                            Status = ApiStatusCode.Status401Unauthorized,
                            Message = "Authentication Failed! Invalid Token."
                        },
                    };
                }
            }
            else if (!isPermission)
            {
                string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                if (!actionName.Equals("LOGOUT", StringComparison.CurrentCultureIgnoreCase))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Bad Request";
                    context.Result = new JsonResult("Forbidden")
                    {
                        Value = new
                        {
                            Status = ApiStatusCode.Status403Forbidden,
                            Message = "You don't have permission to access this feature."
                        },
                    };
                }
            }
        }
        public string GetTokenValue(IPrincipal user, string climType)
        {
            return EncryptionUtility.Decrypt((user.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == climType).Value, AppConfig.AESKeys.Key, AppConfig.AESKeys.IV);
        }
    }
}
