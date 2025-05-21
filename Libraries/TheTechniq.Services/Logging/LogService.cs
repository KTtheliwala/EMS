using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using System.Collections.Generic;
using System.Data;
using LinqToDB.Data;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Data;
using LinqToDB.Linq.Builder;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Data.DataProviders;
using Serilog;
using TheTecniQ.Services.Extensions;

namespace TheTecniQ.Services.Logging
{
    /// <summary>
    /// Log service
    /// </summary>
    public partial class LogService(IRepository<Logs> logRepository) : ILogService
    {
        #region Fields

        private readonly IRepository<Logs> _logRepository = logRepository;

        #endregion

        #region Methods

        public virtual async Task<IPagedList<Logs>> GetAll(GridRequestModel objGrid)
        {
            SearchGrid EntityId = objGrid?.Filters?.Find(x => x.FieldName.Equals("entityid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid ActionById = objGrid?.Filters?.Find(x => x.FieldName.Equals("actionbyid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid CreatedOn = objGrid?.Filters?.Find(x => x.FieldName.Equals("timestamp", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid LogTypeId = objGrid?.Filters?.Find(x => x.FieldName.Equals("logtypeid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid LogSourceId = objGrid?.Filters?.Find(x => x.FieldName.Equals("logsourceid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid EntityName = objGrid?.Filters?.Find(x => x.FieldName.Equals("entityname", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid Message = objGrid?.Filters?.Find(x => x.FieldName.Equals("message", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid Properties = objGrid?.Filters?.Find(x => x.FieldName.Equals("properties", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid ActionId = objGrid?.Filters?.Find(x => x.FieldName.Equals("actionid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            string FirstDate = CreatedOn.FieldValue?.Split('-')[0] ?? "";
            string SecondDate;
            if ((CreatedOn.FieldValue?.Contains('-') ?? false))
            {
                SecondDate = CreatedOn.FieldValue != null ? CreatedOn.FieldValue?.Split('-')[1] ?? "" : "";
            }
            else
            {
                SecondDate = "";
            }

            DateTime? FromDate = !string.IsNullOrEmpty(FirstDate) ? Convert.ToDateTime(FirstDate) : null; //.ToLocalDateTime(objGrid.Timezone)
            DateTime? ToDate = !string.IsNullOrEmpty(SecondDate) ? Convert.ToDateTime(SecondDate).AddHours(23).AddMinutes(59).AddSeconds(59) : null; //.ToLocalDateTime(objGrid.Timezone)

            MsSqlDataProvider obj = new();
            DataParameter[] db =
            [
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@FromDate", Value = FromDate },
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@ToDate", Value = ToDate },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@EntityId", Value = (!string.IsNullOrEmpty(EntityId.FieldValue) ? Convert.ToInt32(EntityId.FieldValue) : null) },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@UserId", Value = (!string.IsNullOrEmpty(ActionById.FieldValue) ? Convert.ToInt32(ActionById.FieldValue) : null) },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@LogTypeId", Value = (!string.IsNullOrEmpty(LogTypeId.FieldValue) ? Convert.ToInt32(LogTypeId.FieldValue) : null) },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@LogSourceId", Value = (!string.IsNullOrEmpty(LogSourceId.FieldValue) ? Convert.ToInt32(LogSourceId.FieldValue) : null) },
                new DataParameter() { DataType = LinqToDB.DataType.NVarChar, Name = "@EntityName", Value = (EntityName.FieldValue ?? "") },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@ActionId", Value = (!string.IsNullOrEmpty(ActionId.FieldValue) ? Convert.ToInt32(ActionId.FieldValue) : null) },
                new DataParameter() { DataType = LinqToDB.DataType.NVarChar, Name = "@Message", Value = (Message.FieldValue ?? "") },
                new DataParameter() { DataType = LinqToDB.DataType.NVarChar, Name = "@Properties", Value = (Properties.FieldValue ?? "") },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@Start", Value = objGrid?.First??0 },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@Length", Value = objGrid?.Rows??10 },
                new DataParameter() { DataType = LinqToDB.DataType.NVarChar, Name = "@SortBy", Value = (objGrid?.SortField ?? "") },
                new DataParameter() { DataType = LinqToDB.DataType.NVarChar, Name = "@OrderBy", Value = ((objGrid?.SortOrder??-1) == -1 ? "DESC" : "ASC") },
            ];
            IList<Logs> data = [];
            IPagedList<Logs> List = new PagedList<Logs>(data, 0, 0, 0);
            DataSet ds = await obj.ExecuteStoredProcedureForDataSetAsync("Get_Logs", CommandType.StoredProcedure, true, db);
            if (ds != null && (ds.Tables?.Count ?? 0) > 0 && (ds.Tables?.Count ?? 0) >= 2)
            {
                data = ServiceCommonExtensions.ConvertDataTable<Logs>(ds.Tables[0]);
                List = new PagedList<Logs>(data, objGrid?.First ?? 0, objGrid?.Rows ?? 10, Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecord"] ?? 0));
            }
            return List;
        }

        public virtual List<Logs> GetddlData()
        {
            MsSqlDataProvider obj = new();
            List<Logs> listLog = [];
            DataSet ds = obj.ExecuteLogStoredProcedureForDataSet("Get_EntityName", null);
            if (ds != null && (ds.Tables?.Count ?? 0) > 0)
            {
                listLog = ServiceCommonExtensions.ConvertDataTable<Logs>(ds.Tables[0]);
            }
            return listLog;
        }

        public virtual async Task<Logs> GetLogByIdAsync(int id)
        {
            return await _logRepository.GetLogByIdAsync(id);

        }
        public virtual void InsertAuditAsync(Logs entity, EnumLogAction action, int UserId, string Username)
        {
            _logRepository.InsertAuditLog(entity, action, UserId, Username);
        }
        public virtual void InsertAuditAsync(IList<Logs> entities, EnumLogAction action, int UserId, string Username)
        {
            _logRepository.InsertAuditLog(entities, action, UserId, Username);
        }
        public virtual void InsertErrorLogAsync(Exception ex, string ErrorMessage, int UserId, string Username)
        {
            _logRepository.InsertErrorLogAsync(ex, ErrorMessage, UserId, Username);
        }
        public virtual void InsertInformationLogAsync(string Message, int UserId, string Username)
        {
            _logRepository.InsertInformationLogAsync(Message, UserId, Username);
        }
        public virtual void InsertWarningLogAsync(string Message, int UserId, string Username)
        {
            _logRepository.InsertWarningLogAsync(Message, UserId, Username);
        }
        #endregion
    }
}