using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;

using TheTecniQ.Core.Domain.User;
using TheTecniQ.API.Infrastructure.Extensions;

namespace TheTecniQ.API.Infrastructure
{
    public static class CurrentContext
    {
        /// <summary>
        /// Get current (logged-in) user
        /// </summary>
        /// <returns>User</returns>
        public static EMS_User User
        {
            get
            {
                return GetCurrentUser();
            }
        }

        //Permissions

        //Timezone

        //Currency

        //Country

        //Language

        public static HttpContext HttpContextAccessor => new HttpContextAccessor().HttpContext;

        private static EMS_User GetCurrentUser()
        {
            ClaimsPrincipal currentUser = HttpContextAccessor.User;

            if (currentUser != null)
            {
                EMS_User user = new();

                if (currentUser.HasClaim(c => c.Type == "Id"))
                {
                    user.Id = currentUser.Claims.FirstOrDefault(c => c.Type == "Id").Value.Descrypt().ToInt();
                }
                if (currentUser.HasClaim(c => c.Type == "UserName"))
                {
                    user.UserName = currentUser.Claims.FirstOrDefault(c => c.Type == "UserName").Value.Descrypt();
                }
                if (currentUser.HasClaim(c => c.Type == "RoleId"))
                {
                    user.RoleId = currentUser.Claims.FirstOrDefault(c => c.Type == "RoleId").Value.Descrypt().ToInt();
                }
                if (currentUser.HasClaim(c => c.Type == "FirstName"))
                {
                    user.FirstName = currentUser.Claims.FirstOrDefault(c => c.Type == "FirstName").Value.Descrypt();
                }
                if (currentUser.HasClaim(c => c.Type == "LastName"))
                {
                    user.LastName = currentUser.Claims.FirstOrDefault(c => c.Type == "LastName").Value.Descrypt();
                }
                return user;
            }
            return null;
        }
    }
}
