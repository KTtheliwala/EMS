using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Data.DataProviders;
using TheTecniQ.Data.Extensions;

namespace TheTecniQ.Services.Permission
{
    public interface IPermissionService
    {
        Task<IList<EMS_Page>> GetDataForDropdown(bool? IsActive);
        Task<IList<EMS_Page>> GetPageDataForDropdown();

        Task<List<EMS_Page>> GetAllModules(int RoleId);

        Task InsertAsync(IList<Core.Domain.Permissions.EMS_Permission> data, int UserId, string UserName);
        Task<IList<PagePermisson>> GetRoleBasedPermissionData(int RoleId);
    }
}