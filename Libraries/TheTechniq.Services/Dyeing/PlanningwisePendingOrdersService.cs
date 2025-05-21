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
using System.Drawing;
using static LinqToDB.Common.Configuration;

namespace TheTecniQ.Services.Dyeing
{
    public partial class PlanningwisePendingOrdersService() : IPlanningwisePendingOrdersService
    {
        #region Fields
        #endregion



        #region Methods

        #region Get

        public async Task<IPagedList<PlanningwisePendingOrders>> GetAll(MobileGridRequestModel objGrid)
        {
            DateTime startDate = DateTime.Now, endDate = DateTime.Now;string planningNo = "";
            
            string defaultWhere = " AND ISNULL(IsClose, 0) = 0 AND ISNULL(IsPlanDispatched, 0) = 0 AND ROUND(ISNULL(PendingWTCHECK, 0), 3) >= 0";
            SearchGrid StartDate = objGrid?.Filters?.Find(x => x.FieldName.Equals("StartDate", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            if (!string.IsNullOrEmpty(StartDate?.FieldValue))
                startDate = Convert.ToDateTime(StartDate?.FieldValue);

            SearchGrid EndDate = objGrid?.Filters?.Find(x => x.FieldName.Equals("EndDate", StringComparison.CurrentCultureIgnoreCase)) ?? new();
            if (!string.IsNullOrEmpty(EndDate?.FieldValue))
                endDate = Convert.ToDateTime(EndDate?.FieldValue);
            MsSqlDataProvider obj = new();
            DataParameter[] para =
            [
                new() { DataType = LinqToDB.DataType.Int32, Name = "PageNumber", Value = objGrid.PageNumber },
                new() { DataType = LinqToDB.DataType.Int32, Name = "PageSize", Value = objGrid.PageSize },
                new() { DataType = LinqToDB.DataType.NVarChar, Name = "StartDate", Value = startDate.Date.ToString("yyyy-MM-dd") },
                new() { DataType = LinqToDB.DataType.NVarChar, Name = "EndDate", Value = endDate.Date.ToString("yyyy-MM-dd") },
                new() { DataType = LinqToDB.DataType.NVarChar, Name = "DynamicWhere", Value = defaultWhere },
            ];
            IList<PlanningwisePendingOrders> data = [];//await obj.QueryProcAsync<DyeingProductionPlan>("EMS_GetDeyingProductionPlanList", para);
            DataSet ds = await obj.ExecuteStoredProcedureForDataSetAsync("EMS_GetPlanningWisePendingOrder", CommandType.StoredProcedure, false, para);
            IPagedList<PlanningwisePendingOrders> List = new PagedList<PlanningwisePendingOrders>(data, 0, 0, 0);
            if (ds != null && (ds.Tables?.Count ?? 0) > 0 && (ds.Tables?.Count ?? 0) >= 1)
            {
                var Dtsummary = await obj.QueryAsync<DyingPlanningSubNoSummary>(@"select DyingPlanningSubNo,sum(BatchWT) as BatchWT from View_PlanningMachineAssign group by DyingPlanningSubNo", null);
                var FunFillJobCardStock = await obj.QueryAsync<ProductionDetailInward>(@"select ItemId,Grade,SHADENO,sum(NetWeight) as NTWT from View_ProductionDetailInward where isnull(IsUsed,0)=0 group by ItemId,Grade,SHADENO", null);

                data = ServiceCommonExtensions.ConvertDataTable<PlanningwisePendingOrders>(ds.Tables[0]);
                data = data.Where(x => (x.DOdate.Date >= startDate.Date && x.DOdate.Date <= endDate.Date)).ToList();
                
                SearchGrid objplanningNo = objGrid?.Filters?.Find(x => x.FieldName.Equals("DyingPlanningNo", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(objplanningNo?.FieldValue))                    
                    data = data.Where(x => (x.DyingPlanningSubNo.ToLower().Trim() == objplanningNo?.FieldValue.ToLower().Trim())).ToList();
                SearchGrid obShadeNo = objGrid?.Filters?.Find(x => x.FieldName.Equals("ShadeNo", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(obShadeNo?.FieldValue))
                    data = data.Where(x => (x.Shadeno ?? "").ToLower().Trim().Contains(obShadeNo.FieldValue.ToLower().Trim())).ToList();
                SearchGrid obPartyName = objGrid?.Filters?.Find(x => x.FieldName.Equals("PartyName", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(obPartyName?.FieldValue))
                    data = data.Where(x => (x.PartyName ?? "").ToLower().Trim().Contains(obPartyName.FieldValue.ToLower().Trim())).ToList();                
                SearchGrid obItemName = objGrid?.Filters?.Find(x => x.FieldName.Equals("ItemName", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(obItemName?.FieldValue))
                    data = data.Where(x => (x.ItemName ?? "").ToLower().Trim().Contains(obItemName.FieldValue.ToLower().Trim())).ToList();                   
                SearchGrid obDeliveryName = objGrid?.Filters?.Find(x => x.FieldName.Equals("DeliveryName", StringComparison.CurrentCultureIgnoreCase)) ?? new();
                if (!string.IsNullOrEmpty(obDeliveryName?.FieldValue))
                    data = data.Where(x => (x.DeliveryName??"").ToLower().Trim().Contains(obDeliveryName.FieldValue.ToLower().Trim())).ToList();                

                if (Dtsummary != null && Dtsummary.Count > 0)
                {
                    foreach (var item in data)
                    {
                        item.TDOQTY = 0;
                        var findObj = Dtsummary.Where(x => x.DyingPlanningSubNo == item.DyingPlanningSubNo).FirstOrDefault();
                        if (findObj != null)
                            item.TDOQTY = findObj.BatchWT;

                        if (FunFillJobCardStock != null && FunFillJobCardStock.Count > 0)
                        {
                            var findStock = FunFillJobCardStock.Where(x => x.SHADENO == item.Shadeno && x.ItemId == item.ItemID && x.Grade == item.Grade).FirstOrDefault();
                            if (findStock != null)
                            {
                                item.STOCKWT = findStock.NTWT;
                            }
                        }
                    }
                }
                List = new PagedList<PlanningwisePendingOrders>(data, (objGrid?.PageNumber ?? 0), (objGrid?.PageSize ?? 20), data.Count);
            }
            return List;
        }
        public async Task<(IList<DyingPlanningView>, extraDetail)> GetDetail(string planningSubNo)
        {
            extraDetail objExtra = new extraDetail();
            MsSqlDataProvider obj = new();
            DataTable dt = new DataTable();
            List<string> headerArray = new List<string> { "ShowDyingPlanning",
                "ShowPlaningQty","ShowPlanningdate","ShowPlanningShadeNo","ShowPlanningShadeName","showAutoNo"
                ,"showMCNo","showAssignQty","SHOWProgramDate","ShowJobCardDate","ShowJobCardNo","ShowItemName",
                "ShowShadeNo","ShowShadeName","ShowBatchNo","Showqty","SHOWJOBCARDTracking","ShowPackingQty","ShowDeliveryQty"
            };
            foreach (var item in headerArray)
            {
                dt.Columns.Add(item, typeof(string));
            }



            #region Deatil Click Event
            {
                string _PlanningSubNo = planningSubNo;
                
                DataSet dsassign = await FillDataONLYTABLE("");
                
                //objassgin = null;
                DataSet ds = await FillPlanningDetailSHOW(" And DyingPlanningsubno=" + _PlanningSubNo + "  Order by DyingPlanningDate ");
                dt.Rows.Clear();
                if (ds?.Tables?.Count > 0)
                {
                    objExtra.lblSOID = ds.Tables[0].Rows[0]["SOID"].ToString();
                    objExtra.lblSONo = ds.Tables[0].Rows[0]["SONo"].ToString();
                    objExtra.lblSODate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SODate"].ToString()).ToString("dd/MM/yyyy");
                    objExtra.lblWeight = Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"].ToString()).ToString("0.000");
                    objExtra.lblBalanceWT = Convert.ToDouble(ds.Tables[0].Rows[0]["Planningqty"].ToString()).ToString("0.000");
                    objExtra.lblPartyName = ds.Tables[0].Rows[0]["PartyName"].ToString();
                    objExtra.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                    String _plansubbnoneew = "";
                    Double _PlanningQty = 0, _JobCardQty = 0;
                    Double _PackingWT = 0, _TotalPackingWT = 0;
                    Double _DeliveryWT = 0, _TotalDeliveryWT = 0;
                    dt.Rows.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dt.Rows.Add();

                        dt.Rows[dt.Rows.Count - 1]["ShowDyingPlanning"] = ds.Tables[0].Rows[i]["DyingPlanningSubNo"].ToString();
                        dt.Rows[dt.Rows.Count - 1]["ShowPlaningQty"] = Convert.ToDouble(ds.Tables[0].Rows[i]["Qty"].ToString());
                        dt.Rows[dt.Rows.Count - 1]["ShowPlanningdate"] = Convert.ToDateTime(ds.Tables[0].Rows[i]["dyingPlanningdate"].ToString());
                        dt.Rows[dt.Rows.Count - 1]["ShowPlanningShadeNo"] = ds.Tables[0].Rows[i]["ShadeNo"].ToString();
                        dt.Rows[dt.Rows.Count - 1]["ShowPlanningShadeName"] = ds.Tables[0].Rows[i]["ShadeName"].ToString();
                        _PlanningQty += Convert.ToDouble(ds.Tables[0].Rows[i]["Qty"].ToString());
                        DataSet dsjob = await FillJOBCARDDetailSHOW(" and DyingPlanningSubNo = " + dt.Rows[dt.Rows.Count - 1]["ShowDyingPlanning"]);
                        _plansubbnoneew = dt.Rows[dt.Rows.Count - 1]["ShowDyingPlanning"].ToString();
                        if (dsjob?.Tables?.Count == 0)
                        {
                            dsjob = await FillData("and DyingPlanningSubNo = " + dt.Rows[dt.Rows.Count - 1]["ShowDyingPlanning"].ToString());
                            if (dsjob?.Tables?.Count > 0)
                            {
                                for (int j = 0; j < dsjob.Tables[0].Rows.Count; j++)
                                {
                                    if (j == 0)
                                    {
                                    }
                                    else
                                    {
                                        dt.Rows.Add();
                                    }
                                    dt.Rows[dt.Rows.Count - 1]["showAutoNo"] = dsjob.Tables[0].Rows[j]["AutoNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["showMCNo"] = dsjob.Tables[0].Rows[j]["MachineNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["showAssignQty"] = Convert.ToDouble(dsjob.Tables[0].Rows[j]["assignqty"].ToString());
                                    if (dsjob.Tables[0].Rows[j]["ProgramDate"] == null)
                                        dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = DBNull.Value;
                                    else if (dsjob.Tables[0].Rows[j]["ProgramDate"].ToString() == "")
                                        dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = DBNull.Value;
                                    else
                                    {
                                        if (dsassign != null && dsassign.Tables[0].Rows.Count > 0)
                                        {
                                            DataRow[] dr = dsassign.Tables[0].Select(" DyingPlanningSubNo='" + _plansubbnoneew + "'");
                                            if (dr != null)
                                            {
                                                if (dr.Length > 0)
                                                    dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = Convert.ToDateTime(dr[0]["ProgramDate"].ToString());
                                            }
                                        }
                                        else
                                        {
                                            dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = Convert.ToDateTime(dsjob.Tables[0].Rows[j]["ProgramDate"].ToString());
                                        }


                                    }

                                    if (dt.Rows[dt.Rows.Count - 1]["showAutoNo"].ToString() == "25850")
                                    { }

                                    dt.Rows[dt.Rows.Count - 1]["ShowJobCardDate"] = DBNull.Value;// Convert.ToDateTime(dsjob.Tables[0].Rows[j]["dyingJobDate"].ToString());
                                    dt.Rows[dt.Rows.Count - 1]["ShowJobCardNo"] = DBNull.Value;// dsjob.Tables[0].Rows[j]["dyingJobNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["ShowItemName"] = DBNull.Value;// dsjob.Tables[0].Rows[j]["FinishItemName"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["ShowShadeNo"] = dsjob.Tables[0].Rows[j]["ShadeNo"].ToString(); ;
                                    dt.Rows[dt.Rows.Count - 1]["ShowShadeName"] = dsjob.Tables[0].Rows[j]["ShadeName"].ToString(); ;
                                    dt.Rows[dt.Rows.Count - 1]["ShowBatchNo"] = dsjob.Tables[0].Rows[j]["BatchNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["Showqty"] = Convert.ToDouble(dsjob.Tables[0].Rows[j]["BatchWT"].ToString()); ;
                                    _JobCardQty += Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Showqty"]);
                                    dt.Rows[dt.Rows.Count - 1]["SHOWJOBCARDTracking"] = null;
                                    dt.Rows[dt.Rows.Count - 1]["ShowPackingQty"] = _PackingWT;
                                    _TotalPackingWT += _PackingWT;
                                    dt.Rows[dt.Rows.Count - 1]["ShowDeliveryQty"] = _DeliveryWT;
                                    _TotalDeliveryWT += _DeliveryWT;
                                }
                            }
                            //objmaaa = null;
                        }
                        else
                        {
                            string _autonolist = "0";
                            if (dsjob.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsjob.Tables[0].Rows.Count; j++)
                                {
                                    if (j == 0)
                                    {
                                    }
                                    else
                                    {
                                        dt.Rows.Add();
                                    }
                                    dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = Convert.ToDateTime(dsjob.Tables[0].Rows[j]["dyingplanningdate"].ToString());

                                    if (dsassign != null && dsassign.Tables[0].Rows.Count > 0)
                                    {
                                        double _atuonosss = Convert.ToDouble(dsjob.Tables[0].Rows[j]["AutoNo"].ToString());
                                        if (_atuonosss == 11905)
                                        { }
                                        DataRow[] dr = dsassign.Tables[0].Select(" DyingPlanningSubNo='" + _plansubbnoneew + "' and AutoNo=" + _atuonosss + " ");
                                        if (dr != null)
                                        {
                                            if (dr.Length > 0)
                                                dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = Convert.ToDateTime(dr[0]["ProgramDate"].ToString());
                                        }
                                    }


                                    dt.Rows[dt.Rows.Count - 1]["ShowJobCardDate"] = Convert.ToDateTime(dsjob.Tables[0].Rows[j]["dyingJobDate"].ToString());
                                    dt.Rows[dt.Rows.Count - 1]["ShowJobCardNo"] = dsjob.Tables[0].Rows[j]["dyingJobNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["ShowItemName"] = dsjob.Tables[0].Rows[j]["FinishItemName"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["ShowShadeNo"] = dsjob.Tables[0].Rows[j]["ShadeNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["ShowShadeName"] = dsjob.Tables[0].Rows[j]["ShadeName"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["ShowBatchNo"] = dsjob.Tables[0].Rows[j]["BatchNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["Showqty"] = Convert.ToDouble(dsjob.Tables[0].Rows[j]["Weight"].ToString());
                                    dt.Rows[dt.Rows.Count - 1]["SHOWJOBCARDTracking"] = dsjob.Tables[0].Rows[j]["JOBCARDTracking"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["showAutoNo"] = dsjob.Tables[0].Rows[j]["AutoNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["showMCNo"] = dsjob.Tables[0].Rows[j]["MachineNo"].ToString();
                                    dt.Rows[dt.Rows.Count - 1]["showAssignQty"] = Convert.ToDouble(dsjob.Tables[0].Rows[j]["ANKURNEWassignqty"].ToString());

                                    if (dt.Rows[dt.Rows.Count - 1]["showAutoNo"].ToString() == "25850")
                                    { }

                                    if (_autonolist != "")
                                        _autonolist += ",";
                                    _autonolist += dt.Rows[dt.Rows.Count - 1]["showAutoNo"].ToString();
                                    _JobCardQty += Convert.ToDouble(dsjob.Tables[0].Rows[j]["Weight"].ToString());
                                    _PackingWT = await FillProductionQty(Convert.ToInt32(objExtra.lblSOID), " and DyingJOBID=" + dsjob.Tables[0].Rows[j]["dyingJobID"].ToString() + " ");
                                    if (Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Showqty"]) > 0)
                                        dt.Rows[dt.Rows.Count - 1]["ShowPackingQty"] = _PackingWT;
                                    else
                                    {
                                        _PackingWT = 0;
                                        dt.Rows[dt.Rows.Count - 1]["ShowPackingQty"] = _PackingWT;
                                    }
                                    //dt.Rows[dt.Rows.Count - 1]["ShowPackingQty"] = _PackingWT;
                                    _TotalPackingWT += _PackingWT;
                                    _DeliveryWT = await FillDeliveryQty(Convert.ToInt32(objExtra.lblSOID), " and DyingJobCardNo='" + dsjob.Tables[0].Rows[j]["dyingJobNo"].ToString() + "' ");
                                    dt.Rows[dt.Rows.Count - 1]["ShowDeliveryQty"] = _DeliveryWT;
                                    _TotalDeliveryWT += _DeliveryWT;
                                }
                                _plansubbnoneew = _PlanningSubNo;
                                //clsPlanningMachineAssign objmaaa = new clsPlanningMachineAssign(true);
                                dsjob = await FillData(" and AutoNo not in (" + _autonolist + ") and DyingPlanningSubNo = " + _PlanningSubNo);
                                if (dsjob?.Tables?.Count > 0)
                                {
                                    for (int j = 0; j < dsjob.Tables[0].Rows.Count; j++)
                                    {
                                        //if (j == 0)
                                        //{
                                        //}
                                        //else
                                        //{
                                        dt.Rows.Add();
                                        // }
                                        dt.Rows[dt.Rows.Count - 1]["SHOWJOBCARDTracking"] = null;
                                        dt.Rows[dt.Rows.Count - 1]["showAutoNo"] = dsjob.Tables[0].Rows[j]["AutoNo"].ToString();
                                        dt.Rows[dt.Rows.Count - 1]["showMCNo"] = dsjob.Tables[0].Rows[j]["MachineNo"].ToString();
                                        dt.Rows[dt.Rows.Count - 1]["showAssignQty"] = Convert.ToDouble(dsjob.Tables[0].Rows[j]["assignqty"].ToString());
                                        if (dsjob.Tables[0].Rows[j]["ProgramDate"] == null)
                                            dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = DBNull.Value;
                                        else if (dsjob.Tables[0].Rows[j]["ProgramDate"].ToString() == "")
                                            dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = DBNull.Value;
                                        else
                                        {
                                            if (dsassign != null && dsassign.Tables[0].Rows.Count > 0)
                                            {
                                                double _atuonosss = Convert.ToDouble(dsjob.Tables[0].Rows[j]["AutoNo"].ToString());
                                                DataRow[] dr = dsassign.Tables[0].Select(" DyingPlanningSubNo='" + _plansubbnoneew + "'  and AutoNo=" + _atuonosss + "   ");
                                                if (dr != null)
                                                {
                                                    if (dr.Length > 0)
                                                    {
                                                        dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = Convert.ToDateTime(dr[0]["ProgramDate"].ToString());
                                                        dt.Rows[dt.Rows.Count - 1]["SHOWJOBCARDTracking"] = "Planned";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"] = Convert.ToDateTime(dsjob.Tables[0].Rows[j]["ProgramDate"].ToString());
                                                dt.Rows[dt.Rows.Count - 1]["SHOWJOBCARDTracking"] = "Planned";
                                            }

                                            //dt.Rows[dt.Rows.Count - 1]["SHOWProgramDate"].Value = Convert.ToDateTime(dsjob.Tables[0].Rows[j]["ProgramDate"].ToString());

                                        }
                                        dt.Rows[dt.Rows.Count - 1]["ShowJobCardDate"] = DBNull.Value;// Convert.ToDateTime(dsjob.Tables[0].Rows[j]["dyingJobDate"].ToString());
                                        dt.Rows[dt.Rows.Count - 1]["ShowJobCardNo"] = DBNull.Value;// dsjob.Tables[0].Rows[j]["dyingJobNo"].ToString();
                                        dt.Rows[dt.Rows.Count - 1]["ShowItemName"] = DBNull.Value;// dsjob.Tables[0].Rows[j]["FinishItemName"].ToString();
                                        dt.Rows[dt.Rows.Count - 1]["ShowShadeNo"] = dsjob.Tables[0].Rows[j]["ShadeNo"].ToString(); ;
                                        dt.Rows[dt.Rows.Count - 1]["ShowShadeName"] = dsjob.Tables[0].Rows[j]["ShadeName"].ToString(); ;
                                        dt.Rows[dt.Rows.Count - 1]["ShowBatchNo"] = dsjob.Tables[0].Rows[j]["BatchNo"].ToString();
                                        dt.Rows[dt.Rows.Count - 1]["Showqty"] = Convert.ToDouble(dsjob.Tables[0].Rows[j]["BatchWT"].ToString()); ;
                                        if (Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Showqty"]) > 0)
                                            dt.Rows[dt.Rows.Count - 1]["SHOWJOBCARDTracking"] = "Batch Created";

                                        _JobCardQty += Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Showqty"]);

                                        if (Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Showqty"]) > 0)
                                            dt.Rows[dt.Rows.Count - 1]["ShowPackingQty"] = _PackingWT;
                                        else
                                        {
                                            _PackingWT = 0;
                                            dt.Rows[dt.Rows.Count - 1]["ShowPackingQty"] = _PackingWT;
                                        }
                                        _TotalPackingWT += _PackingWT;
                                        dt.Rows[dt.Rows.Count - 1]["ShowDeliveryQty"] = _DeliveryWT;
                                        _TotalDeliveryWT += _DeliveryWT;
                                    }
                                }
                            }
                        }
                    }
                    
                    //dt.Rows.Add();
                    //dt.Rows[dt.Rows.Count - 1].DefaultCellStyle.Font = new Font(dt.Font, FontStyle.Bold);
                    //dt.Rows[dt.Rows.Count - 1].DefaultCellStyle.ForeColor = System.Drawing.Color.Maroon;
                    objExtra.ShowPlaningQty = _PlanningQty;
                    objExtra.Showqty = _JobCardQty;
                    objExtra.ShowPackingQty = _TotalPackingWT;
                    objExtra.ShowDeliveryQty = _TotalDeliveryWT;
                }
                ds = null;
                obj = null;
                //  if (dt.Rows.Count > 0)
                {
                    //KGBDetails.Visible = true;
                    //KGBDetails.BringToFront();
                }
            }
            #endregion
            List<DyingPlanningView> rtnData = new List<DyingPlanningView>();
            if (dt != null && dt.Rows.Count > 0)
            {
                rtnData = ServiceCommonExtensions.ConvertDataTable<DyingPlanningView>(dt);
            }
            return (rtnData, objExtra);
        }
        #endregion
        public async Task<double> FillProductionQty(int _soid, string whrcond)
        {
            MsSqlDataProvider obj = new();
            string str = " select Round(Isnull(sum(NetWeight),0),2) as Prod from View_ProductionDetailInward where DyingJOBID in (Select DyingJOBID from  tblDyingJobCardPlanningNEW where soid=" + _soid + ")  " + whrcond;
           var strD =  await obj.QueryEntityAsync<decimal>(str, null);
            return Convert.ToDouble(strD);
        }
        public async Task<double> FillDeliveryQty(int _soid, string whrcond)
        {            
            MsSqlDataProvider obj = new();
            string str = " select Round(Isnull(sum(NetWeight),0),2) as Prod from View_DeliveryChallanDetail where 1=1 And SOID=" + _soid + whrcond;
            var strD = await obj.QueryEntityAsync<decimal>(str, null);
            return Convert.ToDouble(strD);
        }
        public async Task<DataSet> FillPlanningDetailSHOW(string whrcond)
        {
            MsSqlDataProvider obj = new();
            string str = "select  * from View_DyingPlanningDetail where 1=1 " + whrcond;
            return await obj.ExecuteStoredProcedureForDataSetAsync22(str,false,null);
        }
        public async Task<DataSet> FillJOBCARDDetailSHOW(string whrcond)
        {
            MsSqlDataProvider obj = new();
            string str = "select  * from View_DyingJobCardPlanningBatchNEW where 1=1 " + whrcond;
            return await obj.ExecuteStoredProcedureForDataSetAsync22(str, false, null);
        }
        public async Task<DataSet> FillData(string whrcond)
        {
            MsSqlDataProvider obj = new();
            string str = "select * from View_PlanningMachineAssign where 1=1  " + whrcond;
            return await obj.ExecuteStoredProcedureForDataSetAsync22(str, false, null);
        }
        public async Task<DataSet> FillDataONLYTABLE(string whrcond)
        {
            MsSqlDataProvider obj = new();
            string str = "select ProgramDate,DyingPlanningSubNo,AutoNo from tblPlanningMachineAssign where 1=1  " + whrcond;
            return await obj.ExecuteStoredProcedureForDataSetAsync22(str, false, null);
        }
        #endregion
    }
}