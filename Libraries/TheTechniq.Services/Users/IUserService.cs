using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Services.Common;

namespace TheTecniQ.Services.Users
{
    public partial interface IUserService:ICommonService<EMS_User>
    {
        new Task<EMS_User> GetByIdAsync(int Id);
        new Task<IList<EMS_User>> GetByIdsAsync(IList<int> Ids);
        new Task<IPagedList<EMS_User>> GetAllAsync(GridRequestModel objGrid);

        Task<EMS_User> CheckLoginAsync(string UserName, string Password);

        Task<EMS_User> GetByEmailAsync(string Email);

        bool CheckTokenIsValidAsync(int UserId, string UserToken);

        Task<IList<EMS_User>> GetInactiveUserAsync(int days);

        Task<EMS_User> GetUserByUserName(string UserName);
        Task GetCurrentLoginStatus(int id);
        Task<IList<EMS_User>> GetDataForDropdown(bool? IsActive);
    }
}