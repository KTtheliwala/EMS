using System;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Data;

namespace TheTecniQ.Services.Users
{
    public class UserVerificationCodeService(IRepository<EMS_UserVerificationCode> userVerificationRepository) : IUserVerificationCodeService
    {
        #region Fields

        private readonly IRepository<EMS_UserVerificationCode> _userVerificationRepository = userVerificationRepository;

        #endregion
        #region Ctor

        #endregion

        #region Methods

        public virtual async Task<EMS_UserVerificationCode> GetVerificationCodeByUserAsync(int UserId, string Code)
        {
            IQueryable<EMS_UserVerificationCode> query = from u in _userVerificationRepository.Table
                                                     where u.UserId == UserId && u.Code.ToLower().TrimStart().TrimEnd() == Code.ToLower().TrimStart().TrimEnd()
                                                     select u;

            return await query?.FirstOrDefaultAsync() ?? new EMS_UserVerificationCode();
        }

        public virtual async Task<EMS_UserVerificationCode> GetUserVerificationCodeAsync(int UserId)
        {
            IQueryable<EMS_UserVerificationCode> query = from u in _userVerificationRepository.Table
                                                     where u.UserId == UserId && u.IsUse == false
                                                     select u;

            return await query?.FirstOrDefaultAsync() ?? new EMS_UserVerificationCode();
        }

        public virtual async Task InsertAsync(EMS_UserVerificationCode user, int UserId, string Username)
        {
            await _userVerificationRepository.InsertAsync(user, UserId, Username);

            /*await _logService.InsertAsync(user.GetAuditLog(LogAction.Add, UserId, Username));*/
        }

        public virtual async Task UpdateAsync(EMS_UserVerificationCode user, int UserId, string Username)
        {
            await _userVerificationRepository.UpdateAsync(user, UserId, Username);

            /*await _logService.InsertAsync(user.GetAuditLog(LogAction.Edit, UserId, Username));*/
        }

        public virtual async Task<EMS_UserVerificationCode> ResetVerificationCodeAsync(int verificationUserId, int userId, string userName, int expMinutes)
        {
            //TODO : mail shd not send from this method

            EMS_UserVerificationCode objCode = await GetUserVerificationCodeAsync(verificationUserId);

            objCode.Code = (objCode.Id > 0 ? objCode.Code : (new Random().Next(1, 999999)).ToString());
            objCode.UserId = verificationUserId;
            objCode.IsUse = false;
            objCode.CreateDate = DateTime.UtcNow;
            objCode.ExpireDate = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expMinutes) > 0 ? Convert.ToInt32(expMinutes) : 30);
            if (objCode.Id == 0)
            {
                await InsertAsync(objCode, userId, userName);
            }
            else
            {
                await UpdateAsync(objCode, userId, userName);
            }

            return objCode;
        }

        #endregion

    }
}
