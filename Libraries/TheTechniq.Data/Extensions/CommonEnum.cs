using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTecniQ.Data.Extensions
{
    public enum RechargeStaus
    {
        Deposite = 1,
        Withdrawal = 2
    }
    public enum RechargeStausWithCarryForward
    {
        Deposite = 1,
        Withdrawal = 2,
        [Description("Carry Forward")]
        CarryForward = 3
    }
    public enum BidStatus
    {
        Pending = 0,
        Win =1,
        Lost = 2
    }
}
