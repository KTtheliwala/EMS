using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration.UserSecrets;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Core.Domain.Settings;
using TheTecniQ.Data;
using TheTecniQ.Services.Extensions;
using TheTecniQ.Services.Logging;

namespace TheTecniQ.Services.Common
{
    /// <summary>
    /// Setting service
    /// </summary>
    public partial class SystemSettingService(IMemoryCache cache, IRepository<EMS_Settings> syssettingRepository, ILogService logService) : ISystemSettingService
    {
        #region Fields
        private readonly IRepository<EMS_Settings> _syssettingRepository = syssettingRepository;
        private readonly IMemoryCache _cache = cache;
        private readonly ILogService _logService = logService;
        private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(relative: TimeSpan.FromHours(5)) // 5 hours cache
               .SetSize(size: 1024);
        private const string CacheKey = "cache.setting.key";

        #endregion
        #region Methods
        public void RemoveCache()
        {
            _cache.Remove(CacheKey);
        }
        public virtual async Task<IPagedList<EMS_Settings>> GetAllAsync(GridRequestModel objGrid)
        {
            IQueryable<EMS_Settings> query = from ss in _syssettingRepository.Table
                                         where ss.IsActive
                                         select new EMS_Settings()
                                         {
                                             Id = ss.Id,
                                             Key = ss.Key,
                                             Value = ss.Value
                                         };
            return await _syssettingRepository.GetAllPagedAsync(objGrid, query);
        }
        public virtual async Task<List<EMS_Settings>> GetAllAsync()
        {
            IList<EMS_Settings> result = await _syssettingRepository.GetAllAsync(query =>
            {
                return query.Where(x => !string.IsNullOrEmpty(x.Key));
            });
            return [.. result];
        }
        /// <summary>
        /// Gets a System Setting
        /// </summary>
        /// <param name="Id">System Setting id</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the System Setting
        /// </returns>
        public virtual async Task<EMS_Settings> GetByIdAsync(int Id)
        {
            EMS_Settings result = await (from ss in _syssettingRepository.Table
                                     where ss.Id == Id
                                     select new EMS_Settings()
                                     {
                                         Id = ss.Id,
                                         Key = ss.Key,
                                         Value = ss.Value
                                     }).FirstOrDefaultAsync();
            return result;
        }
        public virtual async Task<List<EMS_Settings>> CheckKeyByCountryIdAsync(string key)
        {
            var setting = await _syssettingRepository.GetAllAsync(query => query.Where(x => x.Key == key));
            return [.. setting];
        }
        public virtual async Task<EMS_Settings> GetByKeyAsync(string key)
        {
            var qry = from d in _syssettingRepository.Table
                      where d.Key == key && d.IsActive
                      select d;
            return await qry.FirstOrDefaultAsync();
        }
        public virtual async Task<IList<EMS_Settings>> GetByKeysAsync(string[] keys)
        {
            return await _syssettingRepository.GetAllAsync(query => query.Where(x => keys.Contains(x.Key)));
        }
        /// <summary>
        /// Updates the System Setting
        /// </summary>
        /// <param name="syssetting">System Setting</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        /// 
        public virtual async Task InsertAsync(IList<EMS_Settings> syssetting, int UserId, string Username)
        {
            await _syssettingRepository.InsertAsync(syssetting, UserId, Username);
            // await _logService.AuditAsync(syssetting.GetAuditLog(EnumLogAction.Add, UserId, Username));
        }
        public virtual async Task UpdateAsync(EMS_Settings syssetting, int UserId, string Username)
        {
            RemoveCache();
            await _syssettingRepository.UpdateAsync(syssetting, UserId, Username);
            // await _logService.Insert(syssetting.GetAuditLog(EnumLogAction.Edit, UserId, Username));
        }

        public virtual async Task DeleteAsync(IList<EMS_Settings> syssetting, int UserId, string Username)
        {
            await _syssettingRepository.DeleteAsync(syssetting, UserId, Username);
            // await _logService.AuditAsync(syssetting.GetAuditLog(EnumLogAction.Delete, UserId, Username));
        }

        #endregion
    }
}