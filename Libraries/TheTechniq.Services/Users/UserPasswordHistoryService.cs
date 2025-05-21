using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Data;
using TheTecniQ.Core.Domain.User;

namespace TheTecniQ.Services.Users
{
    public class UserPasswordHistoryService(IRepository<EMS_UserPasswordHistory> userPasswordHistoryRepository) : IUserPasswordHistoryService
    {
        #region Fields

        private readonly IRepository<EMS_UserPasswordHistory> _userPasswordHistoryRepository = userPasswordHistoryRepository;

        #endregion
        #region Ctor

        #endregion

        #region Methods

        public virtual async Task<IList<EMS_UserPasswordHistory>> GetLast5Async(int UserId, int passwordhistory)
        {
            return await _userPasswordHistoryRepository.GetAllAsync(query =>
            {
                return query.Where(x => x.UserId == UserId).OrderByDescending(x => x.CreatedOn).Take(passwordhistory);
            });
        }

        public virtual async Task InsertAsync(EMS_UserPasswordHistory obj, int UserId, string Username)
        {
            await _userPasswordHistoryRepository.InsertAsync(obj,UserId,Username);

            //await _logService.InsertAsync(obj.GetAuditLog(LogAction.Add, UserId, Username));
        }

        #endregion
    }
}
