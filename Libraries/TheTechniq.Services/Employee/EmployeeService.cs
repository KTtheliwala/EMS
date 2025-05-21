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
    public partial class EmployeeService() : IEmployeeService
    {
        #region Fields
        
        #endregion



        #region Methods

        #region Get

        public async Task<IPagedList<tblEmployeeDto>> GetAllAsync(GridRequestModel objGrid)
        {
            string whereClause = "";
            List<string> clauseList = new List<string>();
            SearchGrid EntityId = objGrid?.Filters?.Find(x => x.FieldName.Equals("entityid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid ActionById = objGrid?.Filters?.Find(x => x.FieldName.Equals("actionbyid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid CreatedOn = objGrid?.Filters?.Find(x => x.FieldName.Equals("timestamp", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid LogTypeId = objGrid?.Filters?.Find(x => x.FieldName.Equals("logtypeid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            SearchGrid LogSourceId = objGrid?.Filters?.Find(x => x.FieldName.Equals("logsourceid", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            
            SearchGrid EnrollNo = objGrid?.Filters?.Find(x => x.FieldName.Equals("EnrollNo", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            if (!string.IsNullOrEmpty(EnrollNo?.FieldValue))
                clauseList.Add(" EnrollNo LIKE '%" + EnrollNo?.FieldValue + "%'");

            SearchGrid EmployeeName = objGrid?.Filters?.Find(x => x.FieldName.Equals("EmployeeName", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            if (!string.IsNullOrEmpty(EmployeeName?.FieldValue))
                clauseList.Add(" EmployeeName LIKE '%" + EmployeeName?.FieldValue + "%'");

            SearchGrid IsActive = objGrid?.Filters?.Find(x => x.FieldName.Equals("IsActive", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            if (!string.IsNullOrEmpty(IsActive?.FieldValue))
                clauseList.Add(" IsActive=" + ((IsActive?.FieldValue == "true")?"1":"0") + "");

            if (clauseList.Count > 0)
                whereClause = " WHERE (" + string.Join(" AND ", clauseList) + " ) ";
         
            IList<tblEmployeeDto> data = [];
            IPagedList<tblEmployeeDto> List = new PagedList<tblEmployeeDto>(data, 0, 0, 0);

            MsSqlDataProvider objSql = new();
            var sqlQry = @"select * from View_Employee "+ whereClause + " ORDER BY "+((objGrid?.SortField ?? "EmployeeID")) +" "+ ((objGrid?.SortOrder ?? -1) == -1 ? "DESC" : "ASC") + " OFFSET " + (objGrid?.First ?? 0) + " ROWS FETCH NEXT " + (objGrid?.Rows ?? 10) + " ROWS ONLY";
            data = await objSql.QueryAsync<tblEmployeeDto>(sqlQry, null);
            var dataCount = await objSql.QueryAsync<int>(@"select Count(*) from View_Employee "+ whereClause , null);
            
            if (dataCount != null && (dataCount.Count) > 0 && (dataCount[0]) > 0)
            {   
                List = new PagedList<tblEmployeeDto>(data, objGrid?.First ?? 0, objGrid?.Rows ?? 10, dataCount[0]);
                var EmployeeIDList = List.Select(x => x.EmployeeID).ToList();
                if (EmployeeIDList != null && EmployeeIDList.Count > 0)
                {
                    var dataImageList = await objSql.QueryAsync<tblEmployeeImage>(@"select * from tblEmployeeImage Where EmployeeID IN ("+string.Join(",", EmployeeIDList) +") ", null);
                    if (dataImageList != null && dataImageList.Count > 0)
                    {
                        foreach (var item in List)
                        {
                            var imgObj = dataImageList.FirstOrDefault(c => c.EmployeeID == item.EmployeeID);
                            if (imgObj != null && imgObj.ImageNewID > 0)
                            {
                                item.ImageEmployeePhoto = (imgObj.ImageEmployeePhoto != null && imgObj.ImageEmployeePhoto.Length > 0) ? "data:image/png;base64," + Convert.ToBase64String(imgObj.ImageEmployeePhoto) : null;
                                item.ImageAadharCardFront = (imgObj.ImageAadharCardFront != null && imgObj.ImageAadharCardFront.Length > 0) ? "data:image/png;base64," + Convert.ToBase64String(imgObj.ImageAadharCardFront) : null;
                                item.ImageAadharCardBack = (imgObj.ImageAadharCardBack != null && imgObj.ImageAadharCardBack.Length > 0) ? "data:image/png;base64," + Convert.ToBase64String(imgObj.ImageAadharCardBack) : null;
                                item.ImageOtherDoc = (imgObj.ImageOtherDoc != null && imgObj.ImageOtherDoc.Length > 0) ? "data:image/png;base64," + Convert.ToBase64String(imgObj.ImageOtherDoc) : null;
                            }
                        }
                    }
                }
            }
            return List;

        }

        public async Task<int> InsertAsync(tblEmployee model)
        {
            MsSqlDataProvider objSql = new();

            DataParameter[] parameters =[
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DivisionID", Value = model.DivisionID },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@EmployeeName", Value = model.EmployeeName },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@MobileNo", Value = model.MobileNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Address", Value = model.Address },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DesignationID", Value = model.DesignationID },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@BranchID", Value = model.BranchID },
                new DataParameter() { DataType = LinqToDB.DataType.Boolean, Name = "@IsActive", Value = model.IsActive },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@CreatedBy", Value = model.CreatedBy },
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@CreatedDate", Value = model.CreatedDate },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@EnrollNo", Value = model.EnrollNo },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DepartmentID", Value = model.DepartmentID },
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@DateofBirth", Value = (model.DateofBirth==null)?DateTime.Now:model.DateofBirth },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@Age", Value = model.Age },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@RelativeName", Value = model.RelativeName },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@RelativeMobileNo", Value = model.RelativeMobileNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@ReferenceBy", Value = model.ReferenceBy },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Address2", Value = model.Address2 },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Pincode", Value = model.Pincode },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@RoomNo", Value = model.RoomNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@PhoneNo", Value = model.PhoneNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Email", Value = model.Email },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@BankName", Value = model.BankName },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@BankAccountNo", Value = model.BankAccountNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@PFAccountNo", Value = model.PFAccountNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@NomineeName", Value = model.NomineeName },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@NomineeRelationship", Value = model.NomineeRelationship },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@ESIAccountNo", Value = model.ESIAccountNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@PancardNo", Value = model.PancardNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@AdharcardNo", Value = model.AdharcardNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@DrivingLicNo", Value = model.DrivingLicNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Resident", Value = model.Resident },
                new DataParameter() { DataType = LinqToDB.DataType.DecFloat, Name = "@BasicSalary", Value = model.BasicSalary },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@SalaryType", Value = model.SalaryType },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Sex", Value = model.Sex },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@ACStatus", Value = model.ACStatus },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@VoterID", Value = model.VoterID }
            ];

            var insertedId = await objSql.ExecuteStoredProcedureForInsertedIdAsync("SP_tblEmployeeInsertCustom", CommandType.StoredProcedure, false, "Inserted", parameters);
            return insertedId;

        }
        public async Task UpdateAsync(tblEmployee model)
        {
            MsSqlDataProvider objSql = new();

            DataParameter[] parameters = [

                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@EmployeeID", Value = model.EmployeeID },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DivisionID", Value = model.DivisionID },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@EmployeeName", Value = model.EmployeeName },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@MobileNo", Value = model.MobileNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Address", Value = model.Address },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DesignationID", Value = model.DesignationID },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@BranchID", Value = model.BranchID },
                new DataParameter() { DataType = LinqToDB.DataType.Boolean, Name = "@IsActive", Value = model.IsActive },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@UpdatedBy", Value = model.UpdatedBy },
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@UpdatedDate", Value = model.UpdatedDate },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@EnrollNo", Value = model.EnrollNo },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@DepartmentID", Value = model.DepartmentID },
                new DataParameter() { DataType = LinqToDB.DataType.DateTime, Name = "@DateofBirth", Value = (model.DateofBirth==null)?DateTime.Now:model.DateofBirth },
                new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@Age", Value = model.Age },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@RelativeName", Value = model.RelativeName },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@RelativeMobileNo", Value = model.RelativeMobileNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@ReferenceBy", Value = model.ReferenceBy },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Address2", Value = model.Address2 },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Pincode", Value = model.Pincode },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@RoomNo", Value = model.RoomNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@PhoneNo", Value = model.PhoneNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Email", Value = model.Email },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@BankName", Value = model.BankName },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@BankAccountNo", Value = model.BankAccountNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@PFAccountNo", Value = model.PFAccountNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@NomineeName", Value = model.NomineeName },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@NomineeRelationship", Value = model.NomineeRelationship },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@ESIAccountNo", Value = model.ESIAccountNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@PancardNo", Value = model.PancardNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@AdharcardNo", Value = model.AdharcardNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@DrivingLicNo", Value = model.DrivingLicNo },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Resident", Value = model.Resident },
                new DataParameter() { DataType = LinqToDB.DataType.DecFloat, Name = "@BasicSalary", Value = model.BasicSalary },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@SalaryType", Value = model.SalaryType },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Sex", Value = model.Sex },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@ACStatus", Value = model.ACStatus },
                new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@VoterID", Value = model.VoterID }
            ];

            await objSql.ExecuteStoredForUpdateProcedureAsync("SP_tblEmployeeUpdateCustom", CommandType.StoredProcedure, false, parameters);            

        }
        public async Task<tblEmployee> GetById(int id)
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = @"select * from tblEmployee WHERE EmployeeID ="+ id;
            return await objSql.QueryEntityAsync<tblEmployee>(sqlQry, null);            
        }
        public async Task<IList<tblEmployee>> GetByIds(IList<int> ids)
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = @"select * from tblEmployee WHERE EmployeeID in (" + string.Join(",", ids)+")";
            return await objSql.QueryAsync<tblEmployee>(sqlQry, null);
        }

        public async Task<bool> CheckEnrollNo(string EnrollNo, int? Id)
        {
            MsSqlDataProvider objSql = new();
            var sqlQry = @"select COUNT(0) from tblEmployee WHERE EnrollNo Like '%"+ EnrollNo + "%' AND EmployeeID != "+Id;
            return (await objSql.QueryEntityAsync<int>(sqlQry, null) > 0);
        }






        #endregion

        #endregion
    }
}