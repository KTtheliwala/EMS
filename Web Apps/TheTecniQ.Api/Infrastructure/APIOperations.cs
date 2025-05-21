using Microsoft.AspNetCore.Mvc;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Models.Users;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using System.Threading.Tasks;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;
using System.Linq;
using System;
using DocumentFormat.OpenXml.Spreadsheet;
using TheTecniQ.Core.Domain.Common;
using System.Collections.Generic;
using TheTecniQ.Services.Common;
using Microsoft.Extensions.Configuration.UserSecrets;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace TheTecniQ.Api.Infrastructure
{
    public static class APIOperations
    {
        public static async Task<ApiResponse> Get<T, TModel>(this ICommonService<T> _repo, int id, string NoDataMsg = "No data found.") where T : BaseEntity where TModel : BaseModel
        {
            if (id == 0)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, NoDataMsg);
            }
            T data = await _repo.GetByIdAsync(id);
            return data.ToSingleResponse<T, TModel>("");
        }
        public static async Task<object> List<T>(this ICommonService<T> _repo, GridRequestModel objGrid, string Msg = "List") where T : BaseEntity
        {
            Core.IPagedList<T> RoleList = await _repo.GetAllAsync(objGrid);
            return RoleList.ToListResponse(objGrid, Msg);
        }
        public static async Task<ApiResponse> Post<T, TModel>(this ICommonService<T> _repo, TModel model, Func<IQueryable<T>, IQueryable<T>> isExistQuery = null, int UserId = 0, string Username = "", string isExistMsg = "Role already exist.", string insertMsg = "data added successfully.", string updateMsg = "data updated successfully.") where T : BaseEntity where TModel : BaseModel
        {
            if (isExistQuery != null)
            {
                if (await _repo.IsNameExistAsync(isExistQuery))
                {
                    return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, isExistMsg);
                }
            }
            var ModelData = model.MapTo<T>();
            if (model.Id == 0)
            {
                if (ModelData.GetType().GetProperty("CreatedBy") != null)
                    ModelData.GetType().GetProperty("CreatedBy").SetValue(ModelData, UserId);
                if (ModelData.GetType().GetProperty("MasterCreationDate") != null)
                    ModelData.GetType().GetProperty("MasterCreationDate").SetValue(ModelData, DateTime.UtcNow);
                if (ModelData.GetType().GetProperty("CreatedDate") != null)
                    ModelData.GetType().GetProperty("CreatedDate").SetValue(ModelData, DateTime.UtcNow);
                await _repo.InsertAsync(ModelData, UserId, Username);
                model.Id = ModelData.Id;
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, insertMsg, model);
            }
            else
            {
                T UpdateObj = await _repo.GetByIdAsync(model.Id);
                if (UpdateObj == null)
                {
                    return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "No data found.");
                }
                else
                {
                    if(UpdateObj.GetType().GetProperty("CreatedBy") != null && UpdateObj.GetType().GetProperty("CreatedBy").GetValue(UpdateObj) != null)
                        ModelData.GetType().GetProperty("CreatedBy").SetValue(ModelData, UpdateObj.GetType().GetProperty("CreatedBy").GetValue(UpdateObj));
                    if (UpdateObj.GetType().GetProperty("CreatedDate") != null && UpdateObj.GetType().GetProperty("CreatedDate").GetValue(UpdateObj) != null)
                        ModelData.GetType().GetProperty("CreatedDate").SetValue(ModelData, UpdateObj.GetType().GetProperty("CreatedDate").GetValue(UpdateObj));
                    if (UpdateObj.GetType().GetProperty("MasterCreationDate") != null && UpdateObj.GetType().GetProperty("MasterCreationDate").GetValue(UpdateObj) != null)
                        ModelData.GetType().GetProperty("MasterCreationDate").SetValue(ModelData, UpdateObj.GetType().GetProperty("MasterCreationDate").GetValue(UpdateObj));
                }
                if (ModelData.GetType().GetProperty("ModifyBy") != null)
                    ModelData.GetType().GetProperty("ModifyBy").SetValue(ModelData, UserId);
                if (ModelData.GetType().GetProperty("ModifyDate") != null)
                    ModelData.GetType().GetProperty("ModifyDate").SetValue(ModelData, DateTime.UtcNow);
                await _repo.UpdateAsync(ModelData, UserId, Username);
            }
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, model.Id == 0 ? insertMsg : updateMsg, model);
        }
        public static async Task<ApiResponse> UpdateStatus<T>(this ICommonService<T> _repo, int id, int UserId, string Username, string Msg = "Data updated successfully.", string NoDataMsg = "No data found.") where T : BaseEntity
        {
            if (id == 0)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, NoDataMsg);
            }
            T UpdateObj = await _repo.GetByIdAsync(id);
            if (UpdateObj == null)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, NoDataMsg);
            }
           

            if (UpdateObj.GetType().GetProperty("ModifyBy") != null)
                UpdateObj.GetType().GetProperty("ModifyBy").SetValue(UpdateObj, UserId);            
            if (UpdateObj.GetType().GetProperty("ModifyDate") != null)
                UpdateObj.GetType().GetProperty("ModifyDate").SetValue(UpdateObj, DateTime.UtcNow);
            if (UpdateObj.GetType().GetProperty("IsActive") != null)
            {
                bool IsActive = Convert.ToBoolean(UpdateObj.GetType().GetProperty("IsActive").GetValue(UpdateObj));
                UpdateObj.GetType().GetProperty("IsActive").SetValue(UpdateObj, !IsActive);
            }

            await _repo.UpdateAsync(UpdateObj, UserId, Username);
            return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, Msg);
        }
        public static async Task<ApiResponse> Delete<T>(this ICommonService<T> _repo, IList<int> Ids, int UserId, string Username, string Msg = "Data deleted successfully.", string NoDataMsg = "No data found.") where T : BaseEntity
        {
            if (Ids.Count == 0)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Invalid request parmeters.");
            }
            IList<T> obj = await _repo.GetByIdsAsync(Ids);
            if (obj == null)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, NoDataMsg);
            }
            else
            {
                if (obj != null && obj.Count > 0)
                {
                    foreach (var item in obj)
                    {
                        if (item.GetType().GetProperty("ModifyBy") != null)
                            item.GetType().GetProperty("ModifyBy").SetValue(item, UserId);
                        if (item.GetType().GetProperty("ModifyDate") != null)
                            item.GetType().GetProperty("ModifyDate").SetValue(item, DateTime.UtcNow);
                    }                    
                }
                await _repo.DeleteAsync(obj, UserId, Username);
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, Msg);
            }
        }
    }
}
