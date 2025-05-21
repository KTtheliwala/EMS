using System.Collections.Generic;
using System.Threading.Tasks;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Settings;

namespace TheTecniQ.Services.Common
{
    /// <summary>
    /// System Setting service interface
    /// </summary>
    public partial interface ISystemSettingService
    {
        #region Methods
        Task<IPagedList<EMS_Settings>> GetAllAsync(GridRequestModel objGrid);
        Task<List<EMS_Settings>> GetAllAsync();
        Task<EMS_Settings> GetByIdAsync(int Id);
        Task<List<EMS_Settings>> CheckKeyByCountryIdAsync(string key);
        Task<EMS_Settings> GetByKeyAsync(string key);
        Task<IList<EMS_Settings>> GetByKeysAsync(string[] keys);
        Task InsertAsync(IList<EMS_Settings> syssetting, int UserId, string Username);
        Task UpdateAsync(EMS_Settings syssetting, int UserId, string Username);
        Task DeleteAsync(IList<EMS_Settings> syssetting, int UserId, string Username);
        void RemoveCache();
        #endregion
    }
}