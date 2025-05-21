using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

//using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Data;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Data.DataProviders;
using TheTecniQ.Data.Extensions;
using TheTecniQ.Services.Extensions;
using LinqToDB.Data;
using System;
using TheTecniQ.Core.Domain.Logging;
//using TheTecniQ.Services.Logging;

namespace TheTecniQ.Services.Permission
{
    public partial class PermissionService(IRepository<EMS_Page> PageRepository, IRepository<EMS_Module> moduleRepository,
        IRepository<Core.Domain.Permissions.EMS_Permission> permissionRepository) : IPermissionService
    {
        #region Fields

        private readonly IRepository<EMS_Page> _pageRepository = PageRepository;
        private readonly IRepository<EMS_Module> _moduleRepository = moduleRepository;
        private readonly IRepository<Core.Domain.Permissions.EMS_Permission> _permissionRepository = permissionRepository;

        #endregion

        #region Methods

        public virtual async Task<IList<EMS_Page>> GetDataForDropdown(bool? IsActive)
        {
            return await _pageRepository.GetAllAsync(query =>
            {
                return query.Where(x => (x.IsActive == IsActive || IsActive == null)).Select(y => new EMS_Page { Id = y.Id, TableNames = y.TableNames });
            });
        }
        public virtual async Task<IList<EMS_Page>> GetPageDataForDropdown()
        {
            return await _pageRepository.GetAllAsync(query =>
            {
                return query.Where(x => x.IsActive && x.IsShowSearch && (x.TableNames ?? "") != "").Select(y => new EMS_Page { PageName = y.TableNames, TableNames = y.TableNames }).Distinct();
            });
        }

        public virtual async Task<List<EMS_Page>> GetAllModules(int RoleId)
        {
            IQueryable<EMS_Page> query = from PM in _pageRepository.Table
                                     join M in _moduleRepository.Table on PM.ModuleId equals M.Id
                                     join P in _permissionRepository.Table on
                                     new
                                     {
                                         Key1 = PM.Id,
                                         Key2 = true
                                     }
                                     equals
                                     new
                                     {
                                         Key1 = P.PageId,
                                         Key2 = P.RoleId == RoleId
                                     } into tmpPermission
                                     from P in tmpPermission.DefaultIfEmpty()
                                     where M.IsActive && PM.IsActive
                                     orderby M.Id
                                     select new EMS_Page()
                                     {
                                         ModuleName = M.ModuleName,
                                         PageId = PM.Id,
                                         PageName = PM.PageName,
                                         PageCode = PM.PageCode,
                                         ModuleId = PM.ModuleId,
                                         RoleId = RoleId,
                                         IsAdd = P.IsAdd,
                                         IsDelete = P.IsDelete,
                                         IsEdit = P.IsEdit,
                                         IsView = P.IsView,
                                         PermissionId = P.Id,
                                     };

            return await query.ToListAsync();
        }

        public virtual async Task InsertAsync(IList<Core.Domain.Permissions.EMS_Permission> data, int UserId, string UserName)
        {
            foreach (Core.Domain.Permissions.EMS_Permission item in data)
            {
                IQueryable<Core.Domain.Permissions.EMS_Permission> query = from d in _permissionRepository.Table
                                                                       where d.RoleId == item.RoleId && d.PageId == item.PageId
                                                                       select d;

                if (query.Any())
                {
                    await _permissionRepository.UpdateAsync(item, UserId, UserName);
                }
                else
                {
                    await _permissionRepository.InsertAsync(item, UserId, UserName);
                }
            }
        }
        public virtual async Task<IList<PagePermisson>> GetRoleBasedPermissionData(int RoleId)
        {
            MsSqlDataProvider objSql = new();
            DataParameter[] db = [new DataParameter() { DataType = LinqToDB.DataType.Int32, Name = "@RoleId", Value = RoleId }];
            return await objSql.QueryAsync<PagePermisson>(@"SELECT   M.Id,ISNULL(PP.Id,0) as PermissionId,M.ModuleName,p.Id as PageId,P.PageName,P.PageCode,
                ISNULL(PP.IsAdd,0) IsAdd,ISNULL(PP.IsDelete,0) IsDelete,ISNULL(PP.IsEdit,0) IsEdit,ISNULL(PP.IsView,0) IsView,
                ISNULL(P.IsShowAdd,0) IsShowAdd,ISNULL(P.IsShowDelete,0) IsShowDelete,ISNULL(P.IsShowEdit,0) IsShowEdit,ISNULL(P.IsShowView,0) IsShowView
	                FROM EMS_Module M INNER JOIN [EMS_Page] P ON M.Id=P.ModuleId LEFT JOIN EMS_Permission PP ON P.Id=PP.PageId AND PP.RoleId=@RoleId ORDER BY P.PageName", db);
        }
        #endregion
    }
}