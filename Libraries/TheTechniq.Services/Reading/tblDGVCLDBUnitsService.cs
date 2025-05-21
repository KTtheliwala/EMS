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
using TheTecniQ.Core.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.Xml;
using System.Data;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Data.DataProviders;
using LinqToDB.Data;
using TheTecniQ.Services.Users;
using TheTecniQ.Core.Domain.Masters;
using EasyNetQ;
using System.Net;
using static LinqToDB.Common.Configuration;
using TheTecniQ.Core.Domain.Reading;

namespace TheTecniQ.Services.Reading
{
    public partial class tblDGVCLDBUnitsService() : ItblDGVCLDBUnitsService
    {
        #region Fields

        #endregion



        #region Methods

        #region Get

        

        public async Task<int> InsertAsync(tblDGVCLDBUnits model)
        {
            MsSqlDataProvider objSql = new();

            DataParameter[] parameters = [
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DBDGVCLUnitNo", Value = model.DBDGVCLUnitDate.ToString("yyyyMMdd") },
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@DBDGVCLUnitDate", Value = model.DBDGVCLUnitDate },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DBPanelID", Value = model.DBPanelID },
                new DataParameter() { DataType = LinqToDB.DataType.DecFloat, Name = "@KWH", Value = model.KWH },
                new DataParameter() { DataType = LinqToDB.DataType.DecFloat, Name = "@KVAH", Value = model.KVAH },
                new DataParameter() { DataType = LinqToDB.DataType.DecFloat, Name = "@KVARH", Value = model.KVARH },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@CRBy", Value = model.CRBy },
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@CRDate", Value = model.CRDate }
            ];

            var insertedId = await objSql.ExecuteStoredProcedureForInsertedIdAsync("SP_tblDGVCLDBUnitsInsertCustom", CommandType.StoredProcedure, false, "Inserted", parameters);
            return insertedId;
        }

        public async Task<bool> CheckAlreadyExist(tblDGVCLDBUnits model)
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = $@"SELECT COUNT(0) 
                            FROM tblDGVCLDBUnits 
                            WHERE DBDGVCLUnitDate = '{model.DBDGVCLUnitDate:yyyy-MM-dd HH:mm:ss}' 
                            AND DBDGVCLUnitID != {model.DBDGVCLUnitID}";
            return (await objSql.QueryEntityAsync<int>(sqlQry, null) > 0);
        }

        public async Task<bool> CheckAlreadyExist(tblDBUnits model)
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = $@"SELECT COUNT(0) 
                            FROM tblDBUnits 
                            WHERE DBUnitDate = '{model.DBUnitDate:yyyy-MM-dd HH:mm:ss}' 
                            AND DBUnitID != {model.DBUnitID}";
            return (await objSql.QueryEntityAsync<int>(sqlQry, null) > 0);
        }
        public async Task<int> InsertAsync(tblDBUnits model)
        {
            MsSqlDataProvider objSql = new();

            DataParameter[] parameters = [
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DBUnitNo", Value = model.DBUnitDate.ToString("yyyyMMdd") },
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@DBUnitDate", Value = model.DBUnitDate },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DBID", Value = model.DBID },
                new DataParameter() { DataType = LinqToDB.DataType.DecFloat, Name = "@Units", Value = model.Units },
                new DataParameter() { DataType = LinqToDB.DataType.DecFloat, Name = "@PrevUnits", Value = 0 },
                new DataParameter() { DataType = LinqToDB.DataType.DecFloat, Name = "@UsedUnits", Value = model.Units }
            ];

            var insertedId = await objSql.ExecuteStoredProcedureForInsertedIdAsync("SP_tblDBUnitsInsertCustom", CommandType.StoredProcedure, false, "Inserted", parameters);
            return insertedId;
        }

        public async Task<IList<tblDBMaster>> getTblDBUnits()
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = @"select * from tblDBMaster order by DBName";
            return await objSql.QueryAsync<tblDBMaster>(sqlQry, null);
        }
        public async Task<IList<tblDBPanelMaster>> getTblDBPanelMaster()
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = @"select * from tblDBPanelMaster order by DBName";
            return await objSql.QueryAsync<tblDBPanelMaster>(sqlQry, null);
        }

        public async Task<IList<tblDGVCLDBUnits>> ListTblDGVCLDBUnits(DateTime dt)
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = $@"SELECT * FROM tblDGVCLDBUnits u INNER JOIN 
                            tblDBPanelMaster p on p.DBPanelID = u.DBPanelID
                            WHERE CONVERT(DATE, u.DBDGVCLUnitDate) = '{dt:yyyy-MM-dd}' 
                            ";
            return await objSql.QueryAsync<tblDGVCLDBUnits>(sqlQry, null);
        }

        public async Task<IList<tblDBUnits>> ListTblDBUnits(DateTime dt)
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = $@"SELECT * FROM tblDBUnits u INNER JOIN 
                            tblDBMaster p on p.[DBID] = u.[DBID]
                            WHERE CONVERT(DATE, u.DBUnitDate) = '{dt:yyyy-MM-dd}' 
                            ";
            return await objSql.QueryAsync<tblDBUnits>(sqlQry, null);
        }



        #endregion

        #endregion
    }
}