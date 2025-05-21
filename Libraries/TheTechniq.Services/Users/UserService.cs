using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Data;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Services.Common;
using TheTecniQ.Core.Configuration;
using TheTecniQ.Core.Domain.Notification;

namespace TheTecniQ.Services.Users
{
    public partial class UserService(IRepository<EMS_User> UserRepository, IRepository<EMS_Role> roleRepository) : CommonService<EMS_User>(UserRepository), IUserService
    {
        #region Fields
        private readonly IRepository<EMS_User> _userRepository = UserRepository;
        private readonly IRepository<EMS_Role> _roleRepository = roleRepository;
        #endregion



        #region Methods

        #region Get

        public override async Task<EMS_User> GetByIdAsync(int Id)
        {
            IQueryable<EMS_User> query = from u in _userRepository.Table
                                     join r in _roleRepository.Table on u.RoleId equals r.Id into r_join
                                     from r in r_join.DefaultIfEmpty()
                                     where !u.IsDeleted && u.Id == Id
                                     orderby u.UserName ascending
                                     select new EMS_User()
                                     {
                                         Id = u.Id,
                                         RoleId = u.RoleId,
                                         RoleName = r.Name,
                                         UserName = u.UserName,
                                         Password = u.Password,
                                         FirstName = u.FirstName,
                                         LastName = u.LastName,
                                         Email = u.Email,
                                         Mobile = u.Mobile,
                                         CreatedBy = u.CreatedBy,
                                         CreatedDate = u.CreatedDate,
                                         ModifyBy = u.ModifyBy,
                                         ModifyDate = u.ModifyDate,
                                         IsActive = u.IsActive,
                                         IsDeleted = u.IsDeleted,
                                         Is2FA = u.Is2FA,
                                         TwoFaSecretKey = u.TwoFaSecretKey,
                                         UserToken = u.UserToken,
                                         LastLoggedInOn = u.LastLoggedInOn,
                                         PasswordChangedOn = u.PasswordChangedOn,
                                     };
            return await query.FirstOrDefaultAsync();
        }

        public override async Task<IList<EMS_User>> GetByIdsAsync(IList<int> Ids)
        {
            IQueryable<EMS_User> query = from u in _userRepository.Table
                                     join r in _roleRepository.Table on u.RoleId equals r.Id into r_join
                                     from r in r_join.DefaultIfEmpty()
                                     where !u.IsDeleted && Ids.Contains(r.Id)
                                     orderby u.UserName ascending
                                     select new EMS_User()
                                     {
                                         Id = u.Id,
                                         RoleId = u.RoleId,
                                         RoleName = r.Name,
                                         UserName = u.UserName,
                                         Password = u.Password,
                                         FirstName = u.FirstName,
                                         LastName = u.LastName,
                                         Email = u.Email,
                                         Mobile = u.Mobile,
                                         CreatedBy = u.CreatedBy,
                                         CreatedDate = u.CreatedDate,
                                         ModifyBy = u.ModifyBy,
                                         ModifyDate = u.ModifyDate,
                                         IsActive = u.IsActive,
                                         IsDeleted = u.IsDeleted,
                                         Is2FA = u.Is2FA,
                                         TwoFaSecretKey = u.TwoFaSecretKey,
                                         UserToken = u.UserToken,
                                         LastLoggedInOn = u.LastLoggedInOn,
                                         PasswordChangedOn = u.PasswordChangedOn,
                                     };
            return await query.ToListAsync();
        }

        public override async Task<IPagedList<EMS_User>> GetAllAsync(GridRequestModel objGrid)
        {
            IQueryable<EMS_User> query = from u in _userRepository.Table
                                     join r in _roleRepository.Table on u.RoleId equals r.Id into r_join
                                     from r in r_join.DefaultIfEmpty()
                                     where !u.IsDeleted
                                     orderby u.UserName ascending
                                     select new EMS_User()
                                     {
                                         Id = u.Id,
                                         RoleId = u.RoleId,
                                         RoleName = r.Name,
                                         UserName = u.UserName,
                                         //Password = u.Password,
                                         FirstName = u.FirstName,
                                         LastName = u.LastName,
                                         Email = u.Email,
                                         Mobile = u.Mobile,
                                         CreatedBy = u.CreatedBy,
                                         CreatedDate = u.CreatedDate,
                                         ModifyBy = u.ModifyBy,
                                         ModifyDate = u.ModifyDate,
                                         IsActive = u.IsActive,
                                         IsDeleted = u.IsDeleted,
                                         Is2FA = u.Is2FA,
                                         TwoFaSecretKey = u.TwoFaSecretKey,
                                         UserToken = u.UserToken,
                                         LastLoggedInOn = u.LastLoggedInOn,
                                         PasswordChangedOn = u.PasswordChangedOn,
                                     };

            return await _userRepository.GetAllPagedAsync(objGrid, query);

        }

        public virtual async Task<EMS_User> CheckLoginAsync(string UserName, string Password)
        {
            string p = EncryptionUtility.CreatePasswordHash(Password);

            EMS_User result = await (from u in _userRepository.Table
                                 join r in _roleRepository.Table on u.RoleId equals r.Id into r_join
                                 from r in r_join.DefaultIfEmpty()
                                 where !u.IsDeleted && u.IsActive && u.UserName.ToLower().TrimStart().TrimEnd() == UserName.ToLower().TrimStart().TrimEnd() && u.Password == p
                                 orderby u.Id ascending
                                 select new EMS_User()
                                 {
                                     Id = u.Id,
                                     RoleId = u.RoleId,
                                     RoleName = r.Name,
                                     UserName = u.UserName,
                                     Password = u.Password,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Email = u.Email,
                                     Mobile = u.Mobile,
                                     CreatedBy = u.CreatedBy,
                                     CreatedDate = u.CreatedDate,
                                     ModifyBy = u.ModifyBy,
                                     ModifyDate = u.ModifyDate,
                                     IsActive = u.IsActive,
                                     IsDeleted = u.IsDeleted,
                                     Is2FA = u.Is2FA,
                                     TwoFaSecretKey = u.TwoFaSecretKey,
                                     UserToken = u.UserToken,
                                     LastLoggedInOn = u.LastLoggedInOn,
                                     PasswordChangedOn = u.PasswordChangedOn
                                 }).FirstOrDefaultAsync();
            return result;
        }

        public virtual async Task<EMS_User> GetByEmailAsync(string Email)
        {
            return await (from u in _userRepository.Table
                          where !u.IsDeleted && u.Email.ToLower().TrimStart().TrimEnd() == Email.ToLower().TrimStart().TrimEnd()
                          orderby u.Id ascending
                          select u).FirstOrDefaultAsync();
        }

        public virtual bool CheckTokenIsValidAsync(int UserId, string UserToken)
        {
            var query = from u in _userRepository.Table
                        where u.UserToken == UserToken && u.Id == UserId
                        select u;

            return query.Any();
        }

        public virtual async Task<IList<EMS_User>> GetInactiveUserAsync(int days)
        {
            DateTime lastLogin = DateTime.UtcNow.AddDays(-days);

            var qry = from u in _userRepository.Table
                      where !u.IsDeleted && u.LastLoggedInOn < lastLogin
                      select u;

            return await qry.ToListAsync();
        }

        public async Task GetCurrentLoginStatus(int id)
        {
            var query = from u in _userRepository.Table
                        where u.Id == id
                        select u;

            var data = await  query.FirstOrDefaultAsync();
            if (data != null)
            {
                //data.IsLoginActive = true;
                await _userRepository.UpdateAsync(data, data.Id, data.UserName);
            }

        }
        public virtual async Task<EMS_User> GetUserByUserName(string UserName)
        {
            IQueryable<EMS_User> query = from u in _userRepository.Table
                                     where u.UserName.ToLower().TrimStart().TrimEnd() == UserName.ToLower().TrimStart().TrimEnd() && u.IsDeleted == false
                                     select u;
            return await query?.FirstOrDefaultAsync() ?? new EMS_User();
        }
        public virtual async Task<IList<EMS_User>> GetDataForDropdown(bool? IsActive)
        {
            return await _userRepository.GetAllAsync(query =>
            {
                return query.Where(x => (x.IsActive == IsActive || IsActive == null) && !x.IsDeleted).Select(y => new EMS_User { Id = y.Id, FullName = y.FirstName + " " + y.LastName });
            });
        }
        #endregion

        #endregion
    }
}