using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TheTecniQ.Core.Infrastructure {
    public enum EnumDispatchQtyStatus
    {
        [Description("Dispatch")]
        Dispatch = 1,
        [Description("Cancel")]
        Cancel = 0
    }
    public enum EnumImportDetailStatus
    {
        [Description("Pending")]
        Pending = 0,
        [Description("Half Dispatch")]
        HalfDispatch = 1,
        [Description("Complete")]
        Complete = 2
    }
    public enum EnumOrderStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("In-Progress")]
        InProgress = 1,
        [Description("Ready For Delivery")]
        ReadyForDelivery = 3,
        [Description("Completed")]
        Completed = 4,
        [Description("Cancelled")]
        Cancelled = 5
    }
    public enum EnumDiscountType
    {
        Flate = 1,
        Percentage = 2
    }
    public enum PlatForm
    {
        [Description("Desktop")]
        Desktop = 1,
        [Description("Mobile")]
        Mobile = 2
    }
}
