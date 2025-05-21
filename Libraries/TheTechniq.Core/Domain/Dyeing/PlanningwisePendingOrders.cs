using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTecniQ.Core.Domain.Dyeing
{
    public class PlanningwisePendingOrders
    {
        public string DyingPlanningSubNo { get; set; }
        public int? DyingPlanningSubID { get; set; }
        public DateTime DOdate { get; set; }
        public int? SOID { get; set; }
        public string SONO { get; set; }
        public DateTime? SoDate { get; set; }
        public string Acode { get; set; }
        public decimal? SOQTY { get; set; }
        public string PartyName { get; set; }
        public string DeliveryName { get; set; }
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public string Grade { get; set; }
        public string Shadeno { get; set; }
        public string ShadeName { get; set; }
        public bool? ISSTOCK { get; set; }
        public decimal? PendingWTCHECK { get; set; }
        public decimal? PendingWT { get; set; }
        public decimal? STOCKWT { get; set; }
        public decimal? DeliverQty { get; set; }
        public decimal? Weight { get; set; }
        public int? TDOQTY { get; set; }
        public decimal? DeliveryWeightInChallan { get; set; }
        public decimal? TotalDeliveryQtyinChallan { get; set; }
        public bool? IsCloseManual { get; set; }
        public bool? IsClose { get; set; }
        public bool? IsPlanDispatched { get; set; }
        public bool? ISMOSTURGENT { get; set; }
        public bool? ForStock { get; set; }
        public bool? IsCancel { get; set; }
        public string DeliveryAcode { get; set; }
    }

    public class DyingPlanningSubNoSummary {
        public string DyingPlanningSubNo { get; set; }
        public int? BatchWT { get; set; }
    }

    public class ProductionDetailInward
    {
        public int ItemId { get; set; }
        public string Grade{ get; set; }
        public string SHADENO{ get; set; }
        public decimal? NTWT{ get; set; }
    }
   
    public class extraDetail { 
    public string lblSOID { get; set; }
    public string lblSONo { get; set; }
    public string lblSODate { get; set; }
    public string lblWeight { get; set; }
    public string lblBalanceWT { get; set; }
    public string lblPartyName { get; set; }
    public string ItemName { get; set; }
    public double? ShowPlaningQty { get; set; }
    public double? Showqty { get; set; }
    public double? ShowPackingQty { get; set; }
    public double? ShowDeliveryQty { get; set; }
    }


    public class DyingPlanningView
    {
        public int? ShowDyingPlanning { get; set; }
        public decimal? ShowPlaningQty { get; set; }
        public DateTime? ShowPlanningdate { get; set; }
        public string ShowPlanningShadeNo { get; set; }
        public string ShowPlanningShadeName { get; set; }
        public int? ShowAutoNo { get; set; }
        public string ShowMCNo { get; set; }
        public decimal? ShowAssignQty { get; set; }
        public DateTime? ShowProgramDate { get; set; }
        public DateTime? ShowJobCardDate { get; set; }
        public string ShowJobCardNo { get; set; }
        public string ShowItemName { get; set; }
        public string ShowShadeNo { get; set; }
        public string ShowShadeName { get; set; }
        public string ShowBatchNo { get; set; }
        public decimal? Showqty { get; set; }
        public string ShowJobCardTracking { get; set; }
        public decimal? ShowPackingQty { get; set; }
        public decimal? ShowDeliveryQty { get; set; }
    }
}
