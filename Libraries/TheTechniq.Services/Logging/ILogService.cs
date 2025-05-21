using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Services.Logging
{
    /// <summary>
    /// Log service interface
    /// </summary>
    public partial interface ILogService
    {
        #region Methods
        Task<IPagedList<Logs>> GetAll(GridRequestModel objGrid);
        List<Logs> GetddlData();
        Task<Logs> GetLogByIdAsync(int id);
        void InsertAuditAsync(Logs entity, EnumLogAction action, int UserId, string Username);
        void InsertAuditAsync(IList<Logs> entities, EnumLogAction action, int UserId, string Username);
        void InsertErrorLogAsync(Exception ex, string ErrorMessage, int UserId, string Username);
        void InsertInformationLogAsync(string Message, int UserId, string Username);
        void InsertWarningLogAsync(string Message, int UserId, string Username);
        #endregion
    }
}