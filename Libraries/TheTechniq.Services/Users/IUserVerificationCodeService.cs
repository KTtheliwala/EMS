using System.Threading.Tasks;

using TheTecniQ.Core.Domain.User;

namespace TheTecniQ.Services.Users
{
    public interface IUserVerificationCodeService
    {
        Task<EMS_UserVerificationCode> GetVerificationCodeByUserAsync(int UserId, string Code);
        Task<EMS_UserVerificationCode> GetUserVerificationCodeAsync(int UserId);

        Task InsertAsync(EMS_UserVerificationCode user, int UserId, string Username);
        Task UpdateAsync(EMS_UserVerificationCode user, int UserId, string Username);
        Task<EMS_UserVerificationCode> ResetVerificationCodeAsync(int verificationUserId, int userId, string userName, int expMinutes);
    }
}
