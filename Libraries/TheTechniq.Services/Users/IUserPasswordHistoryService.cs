using System.Collections.Generic;
using System.Threading.Tasks;

using TheTecniQ.Core.Domain.User;

namespace TheTecniQ.Services.Users
{
    public interface IUserPasswordHistoryService
    {
        #region Methods

        Task<IList<EMS_UserPasswordHistory>> GetLast5Async(int UserId, int passwordhistory);

        Task InsertAsync(EMS_UserPasswordHistory obj, int UserId, string Username);

        #endregion
    }
}
