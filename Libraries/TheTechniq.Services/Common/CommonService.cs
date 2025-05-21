using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Data;

namespace TheTecniQ.Services.Common
{
    public partial class CommonService<T>(IRepository<T> commonService) : ICommonService<T> where T : BaseEntity
    {
        #region Fields

        private readonly IRepository<T> _commonService = commonService;

        #endregion
        #region Get
        public virtual async Task<IList<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> func = null)
        {
            return await _commonService.GetAllAsync(func);
        }
        public virtual async Task<IPagedList<T>> GetAllAsync(GridRequestModel objGrid)
        {
            return await _commonService.GetAllPagedAsync(objGrid);
        }

        public virtual async Task<T> GetByIdAsync(int Id)
        {
            return await _commonService.GetByIdAsync(Id);
        }

        public virtual async Task<IList<T>> GetByIdsAsync(IList<int> ids)
        {
            return await _commonService.GetByIdsAsync(ids);
        }

        public virtual async Task<bool> IsNameExistAsync(Func<IQueryable<T>, IQueryable<T>> func)
        {
            IList<T> result = await _commonService.GetAllAsync(func);
            return result.Count > 0;
        }

        #endregion

        #region Insert / Update / Delete

        public virtual async Task<T> InsertAsync(T role, int UserId, string Username)
        {
            return await _commonService.InsertAsync(role, UserId, Username);
        }
        public virtual async Task InsertRangeAsync(IList<T> role, int UserId, string Username)
        {
            await _commonService.InsertAsync(role, UserId, Username);
        }

        public virtual async Task UpdateAsync(T role, int UserId, string Username)
        {
            await _commonService.UpdateAsync(role, UserId, Username);
        }

        public virtual async Task UpdateAsync(IList<T> rolelist, int UserId, string Username)
        {
            await _commonService.UpdateAsync(rolelist, UserId, Username);
        }

        public virtual async Task DeleteAsync(IList<T> rolelist, int UserId, string Username)
        {
            await _commonService.DeleteAsync(rolelist, UserId, Username);
        }

        #endregion
    }
}