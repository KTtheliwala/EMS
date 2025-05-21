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

namespace TheTecniQ.Services.Employees
{
    public partial class EmployeeImagesService() : IEmployeeImagesService
    {
        #region Fields
        
        #endregion



        #region Methods

        #region Get

       
        public async Task<int> InsertAsync(tblEmployeeImage model)
        {
            MsSqlDataProvider objSql = new();

            DataParameter[] parameters =[
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@EmployeeID", Value = model.EmployeeID },
                new DataParameter() { DataType = LinqToDB.DataType.Image, Name = "@ImageEmployeePhoto", Value = model.ImageEmployeePhoto },
                new DataParameter() { DataType = LinqToDB.DataType.Image, Name = "@ImageAadharCardFront", Value = model.ImageAadharCardFront },
                new DataParameter() { DataType = LinqToDB.DataType.Image, Name = "@ImageAadharCardBack", Value = model.ImageAadharCardBack },
                new DataParameter() { DataType = LinqToDB.DataType.Image, Name = "@ImageOtherDoc", Value = model.ImageOtherDoc }
            ];

            var insertedId = await objSql.ExecuteStoredProcedureForInsertedIdAsync("SP_tblEmployeeImageInsert", CommandType.StoredProcedure, false, "Inserted", parameters);
            return insertedId;

        }
        public async Task UpdateAsync(tblEmployeeImage model)
        {
            MsSqlDataProvider objSql = new();

            DataParameter[] parameters = [

                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@EmployeeID", Value = model.EmployeeID },
                new DataParameter() { DataType = LinqToDB.DataType.Image, Name = "@ImageEmployeePhoto", Value = model.ImageEmployeePhoto },
                new DataParameter() { DataType = LinqToDB.DataType.Image, Name = "@ImageAadharCardFront", Value = model.ImageAadharCardFront },
                new DataParameter() { DataType = LinqToDB.DataType.Image, Name = "@ImageAadharCardBack", Value = model.ImageAadharCardBack },
                new DataParameter() { DataType = LinqToDB.DataType.Image, Name = "@ImageOtherDoc", Value = model.ImageOtherDoc }                
            ];

            await objSql.ExecuteStoredForUpdateProcedureAsync("SP_tblEmployeeImageUpdate", CommandType.StoredProcedure, false, parameters);            

        }
        public async Task<tblEmployeeImage> GetById(int id)
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = @"select * from tblEmployeeImage WHERE EmployeeID =" + id;
            return await objSql.QueryEntityAsync<tblEmployeeImage>(sqlQry, null);            
        }






        #endregion

        #endregion
    }
}