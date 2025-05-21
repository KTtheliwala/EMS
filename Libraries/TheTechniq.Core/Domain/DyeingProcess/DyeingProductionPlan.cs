using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTecniQ.Core.Domain.DyeingProcess
{
    public class DyeingProductionPlan
    {
        public string JOBCARDTracking { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public string ShadeNo { get; set; }
        public string ShadeName { get; set; }
        public string MachineNo { get; set; }
        public string MachineName { get; set; }
        public int? AutoID { get; set; }
        public int? AutoNo { get; set; }
        public DateTime? AutoDate { get; set; }
        public int? DyingPlanningSubNo { get; set; }
        public int? MachineID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal? AssignQty { get; set; }
        public decimal? PlanningQty { get; set; }
        public string BatchNo { get; set; }
        public int? ProgramPriority { get; set; }
        public decimal? BatchWT { get; set; }
        public DateTime? ProgramDate { get; set; }
        public decimal? OrderQtyForSO { get; set; }
        public int? ACode { get; set; }
        public int? ItemID { get; set; }
        public string COPartyName { get; set; }
        public string DeliveryPartyName { get; set; }
        public string DLNo { get; set; }
        public string Remarks { get; set; }
        public string MachineAssignRemarks { get; set; }
        public DateTime? DyingPlanningDate { get; set; }
        public int? ProcessChartNo { get; set; }
        public int? BatchCreationID { get; set; }
        public int? TDO { get; set; }
        public decimal? NewPlanOrderQty { get; set; }
        public bool? IsMostUrgent { get; set; }
        public bool? IsManual { get; set; }
        public bool? IsCancel { get; set; }
        public int? MachineOrder { get; set; }
        public string ItemCategory { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? ConsigneeID { get; set; }
    }
    public class MachineProgramGroup
    {
        public string MachineNo { get; set; }
        public DateTime? ProgramDate { get; set; }
        public List<DyeingProductionPlan> Items { get; set; }
    }
    public class DyeingProductionPlanSummary {
        public string ShadeNo { get; set; }
        public int? AssignQty { get; set; }
    }
}
