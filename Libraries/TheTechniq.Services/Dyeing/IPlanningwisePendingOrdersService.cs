using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Dyeing;
using TheTecniQ.Core.Domain.DyeingProcess;
using TheTecniQ.Core.Domain.Employees;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Services.Common;

namespace TheTecniQ.Services.Dyeing
{
    public partial interface IPlanningwisePendingOrdersService
    {
        Task<IPagedList<PlanningwisePendingOrders>> GetAll(MobileGridRequestModel objGrid);

        Task<(IList<DyingPlanningView>, extraDetail)> GetDetail(string planningSubNo);

    }
}