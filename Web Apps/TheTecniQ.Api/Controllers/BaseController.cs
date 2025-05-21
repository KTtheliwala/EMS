using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Asp.Versioning;

using TheTecniQ.API.Infrastructure.Extensions;

namespace TheTecniQ.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class BaseController : ControllerBase
    {
        public ClaimsPrincipal CurrentUser => HttpContext.User;

        public int CurrentUserId
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "Id"))
                {
                    return CurrentUser.Claims.FirstOrDefault(c => c.Type == "Id").Value.Descrypt().ToInt();
                }
                else
                {
                    return 0;
                }
            }
        }

        public string CurrentUserName
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "UserName"))
                {
                    return CurrentUser.Claims.FirstOrDefault(c => c.Type == "UserName").Value.Descrypt();
                }
                else
                {
                    return "";
                }
            }
        }

        public int CurrentRoleId
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "RoleId"))
                {
                    return CurrentUser.Claims.FirstOrDefault(c => c.Type == "RoleId").Value.Descrypt().ToInt();
                }
                else
                {
                    return 0;
                }
            }
        }

        public int CountryId
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "CountryId"))
                {
                    return CurrentUser.Claims.FirstOrDefault(c => c.Type == "CountryId").Value.Descrypt().ToInt();
                }
                else
                {
                    return 0;
                }
            }
        }

        public string CountryName
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "CountryName"))
                {
                    return CurrentUser.Claims.FirstOrDefault(c => c.Type == "CountryName").Value.Descrypt();
                }
                else
                {
                    return "";
                }
            }
        }

        public string CurrentUserFullName
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "FullName"))
                {
                    return CurrentUser.Claims.FirstOrDefault(c => c.Type == "FullName").Value.Descrypt();
                }
                else
                {
                    return "";
                }
            }
        }

        public string CurrentUserOffSet
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "Offset"))
                {
                    return CurrentUser.Claims.FirstOrDefault(c => c.Type == "Offset").Value.Descrypt();
                }
                else
                {
                    return "+8:00";
                }
            }
        }
    }
}
