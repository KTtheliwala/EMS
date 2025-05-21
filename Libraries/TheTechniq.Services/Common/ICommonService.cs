using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;

namespace TheTecniQ.Services.Common
{
    public partial interface ICommonService<T> where T : class
    {
        Task<IList<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> func=null);
        Task<IPagedList<T>> GetAllAsync(GridRequestModel objGrid);
        Task<T> GetByIdAsync(int Id);

        Task<IList<T>> GetByIdsAsync(IList<int> ids);

        Task<bool> IsNameExistAsync(Func<IQueryable<T>, IQueryable<T>> func);


        //Insert / Update / Delete Methods

        Task<T> InsertAsync(T role, int UserId, string Username);
        Task InsertRangeAsync(IList<T> role, int UserId, string Username);

        Task UpdateAsync(T role, int UserId, string Username);

        Task UpdateAsync(IList<T> rolelist, int UserId, string Username);

        Task DeleteAsync(IList<T> rolelist, int UserId, string Username);
    }
}