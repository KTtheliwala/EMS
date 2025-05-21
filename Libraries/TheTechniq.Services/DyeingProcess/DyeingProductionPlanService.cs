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
using TheTecniQ.Core.Domain.Dyeing;
using TheTecniQ.Core.Domain.DyeingProcess;
using System.Globalization;
using System.Collections;

namespace TheTecniQ.Services.DyeingProcess
{
    public partial class DyeingProductionPlanService() : IDyeingProductionPlanService
    {
        #region Fields
        #endregion



        #region Methods

        #region Get

        public async Task<IPagedList<MachineProgramGroup>> GetAll(MobileGridRequestModel objGrid)
        {
            
            DateTime? startDate = null, endDate = null;
            string defaultWhere = " AND ISNULL(ProcessChartNo, 0) = 0";
            SearchGrid StartDate = objGrid?.Filters?.Find(x => x.FieldName.Equals("StartDate", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            if (!string.IsNullOrEmpty(StartDate?.FieldValue))
                startDate = Convert.ToDateTime(StartDate?.FieldValue);

            SearchGrid EndDate = objGrid?.Filters?.Find(x => x.FieldName.Equals("EndDate", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            if (!string.IsNullOrEmpty(EndDate?.FieldValue))
                endDate = Convert.ToDateTime(EndDate?.FieldValue);

            bool IsCompleted = false;
            var obIsCompleted = objGrid?.Filters?.FirstOrDefault(x =>
                x.FieldName.Equals("IsCompleted", StringComparison.CurrentCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(obIsCompleted?.FieldValue) &&
                (obIsCompleted.FieldValue == "true" || obIsCompleted.FieldValue == "1"))
            {
                IsCompleted = true;
            }

            bool IsPending = false;
            var obIsPending = objGrid?.Filters?.FirstOrDefault(x =>
                x.FieldName.Equals("IsPending", StringComparison.CurrentCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(obIsPending?.FieldValue) &&
                (obIsPending.FieldValue == "true" || obIsPending.FieldValue == "1"))
            {
                IsPending = true;
            }

            bool IsProcess = false;
            var obIsProcess = objGrid?.Filters?.FirstOrDefault(x =>
                x.FieldName.Equals("IsProcess", StringComparison.CurrentCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(obIsProcess?.FieldValue) &&
                (obIsProcess.FieldValue == "true" || obIsProcess.FieldValue == "1"))
            {
                IsProcess = true;
            }
            string whrcond = "";
            if (IsPending)
            {
                if (IsProcess)
                {
                    whrcond += " and (isnull(ProcessChartNo,0)=0  or DyingPlanningSubNo in ( select DyingPlanningSubNo from View_DyingJobCardPlanningBatchNEW where JOBCARDTracking='Process') ) ";
                }
                else
                {
                    whrcond += " and  isnull(ProcessChartNo,0)=0  ";
                }
            }
            if (IsCompleted)
            {
                whrcond += " and (isnull(BatchNo,'')!='' and isnull(ProcessChartNo,0)!=0 ) ";
            }
            if (whrcond == "")
                whrcond = defaultWhere;
            MsSqlDataProvider obj = new();
            DataParameter[] para =
            [
                new() { DataType = LinqToDB.DataType.Int32, Name = "PageNumber", Value = objGrid.PageNumber },
                new() { DataType = LinqToDB.DataType.Int32, Name = "PageSize", Value = objGrid.PageSize },
                new() { DataType = LinqToDB.DataType.NVarChar, Name = "StartDate", Value = (startDate == null)?null:Convert.ToDateTime(startDate).Date.ToString("yyyy-MM-dd") },
                new() { DataType = LinqToDB.DataType.NVarChar, Name = "EndDate", Value = (endDate == null)?null:Convert.ToDateTime(endDate).Date.ToString("yyyy-MM-dd") },
                new() { DataType = LinqToDB.DataType.NVarChar, Name = "DynamicWhere", Value = whrcond },
            ];
            IList<DyeingProductionPlan> data = [];//await obj.QueryProcAsync<DyeingProductionPlan>("EMS_GetDeyingProductionPlanList", para);
            IList<MachineProgramGroup> data1 = [];
            DataSet ds = await obj.ExecuteStoredProcedureForDataSetAsync("EMS_GetDeyingProductionPlanList", CommandType.StoredProcedure, false, para);
            IPagedList<DyeingProductionPlan> List = new PagedList<DyeingProductionPlan>(data, 0, 0, 0);
            DataSet ds1 = new DataSet();
            IPagedList<MachineProgramGroup> List1 = new PagedList<MachineProgramGroup>(data1, 0, 0, 0);
            if (ds != null && (ds.Tables?.Count ?? 0) > 0 && (ds.Tables?.Count ?? 0) >= 1)
            {
                data = ServiceCommonExtensions.ConvertDataTable<DyeingProductionPlan>(ds.Tables[0]);
                var Dtsummary = await obj.QueryAsync<DyeingProductionPlanSummary>(@"select sum(assignqty) as assignqty,SHADENO from View_PlanningMachineAssign as pm where 1=1  group by SHADENO Order by SHADENO", null);

                data.ToList().ForEach(x => x.DeliveryPartyName = x.COPartyName);

                SearchGrid objMcNo = objGrid?.Filters?.Find(x => x.FieldName.Equals("MCNo", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(objMcNo?.FieldValue))
                    data = data.Where(x => (x.MachineNo.ToLower().Trim() == objMcNo?.FieldValue.ToLower().Trim())).ToList();
                SearchGrid obShadeNo = objGrid?.Filters?.Find(x => x.FieldName.Equals("ShadeNo", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(obShadeNo?.FieldValue))
                    data = data.Where(x => (x.ShadeNo ?? "").ToLower().Trim().Contains(obShadeNo.FieldValue.ToLower().Trim())).ToList();
                SearchGrid obPartyName = objGrid?.Filters?.Find(x => x.FieldName.Equals("PartyName", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(obPartyName?.FieldValue))
                    data = data.Where(x => (x.PartyName ?? "").ToLower().Trim().Contains(obPartyName.FieldValue.ToLower().Trim())).ToList();
                SearchGrid obShadeName = objGrid?.Filters?.Find(x => x.FieldName.Equals("ShadeName", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(obShadeName?.FieldValue))
                    data = data.Where(x => (x.ShadeName ?? "").ToLower().Trim().Contains(obShadeName.FieldValue.ToLower().Trim())).ToList();
                SearchGrid obItemName = objGrid?.Filters?.Find(x => x.FieldName.Equals("ItemName", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(obItemName?.FieldValue))
                    data = data.Where(x => (x.ItemName ?? "").ToLower().Trim().Contains(obItemName.FieldValue.ToLower().Trim())).ToList();
                

                if (Dtsummary != null && Dtsummary.Count > 0)
                {
                    foreach (var item in data)
                    {
                        //item.PartyName = item.PartyName + Environment.NewLine + item.COPartyName;
                        item.TDO = 0;
                        var findObj = Dtsummary.Where(x => x.ShadeNo == item.ShadeNo).FirstOrDefault();
                        if (findObj != null)
                            item.TDO = findObj?.AssignQty;
                    }
                }
                var groupedList = data
    .Where(x => x.MachineNo != null)
    .GroupBy(x =>
        Tuple.Create(
            x.MachineNo,
            x.ProgramDate.HasValue ? x.ProgramDate.Value.Date : (DateTime?)null
        )
    )
    .Select(g => new MachineProgramGroup
    {
        MachineNo = g.Key.Item1,
        ProgramDate = g.Key.Item2,
        Items = g.OrderBy(x => x.ProgramPriority).ToList()
    })
    .OrderBy(g => g.ProgramDate.HasValue ? 1 : 0)  // Nulls first
    .ThenBy(g => g.ProgramDate)
    .ThenBy(g => g.MachineNo)
    .ToList();

                List1 = new PagedList<MachineProgramGroup>(groupedList, (objGrid?.PageNumber ?? 0), (objGrid?.PageSize ?? 20), data.Count);
            }
            return List1;
        }

        #endregion

        #endregion
    }
}