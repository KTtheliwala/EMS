using TheTecniQ.Core.Domain.Common;

namespace TheTecniQ.Core.Domain.Permissions
{
    public class EMS_Page : BaseEntity
    {
        public int ModuleId { get; set; }

        public string PageName { get; set; }

        public string PageCode { get; set; }

        public bool IsShowMenu { get; set; }

        public bool IsShowSearch { get; set; }

        public string TableNames { get; set; }

        public bool IsShowAdd { get; set; }
        public bool IsShowEdit { get; set; }
        public bool IsShowDelete { get; set; }
        public bool IsShowView { get; set; }
        public bool IsActive { get; set; } = true;
        [NoColumnMap]
        public string ModuleName { get; set; }
        [NoColumnMap]
        public int PageId { get; set; }
        [NoColumnMap]
        public int RoleId { get; set; }
        [NoColumnMap]
        public bool IsView { get; set; }
        [NoColumnMap]

        public bool IsAdd { get; set; }
        [NoColumnMap]

        public bool IsEdit { get; set; }
        [NoColumnMap]

        public bool IsDelete { get; set; }
        [NoColumnMap]
        public int PermissionId { get; set; }
    }

    //Non entities
    public class PagePermisson : NoMapBaseEntity
    {
        public int PermissionId { get; set; }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }

        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageCode { get; set; }

        public bool IsView { get; set; }

        public bool IsAdd { get; set; }

        public bool IsEdit { get; set; }

        public bool IsDelete { get; set; }
        [NoColumnMap]
        public bool IsShowMenu { get; set; }
        [NoColumnMap]
        public bool IsShowSearch { get; set; }
        [NoColumnMap]
        public bool IsShowAdd { get; set; }
        [NoColumnMap]
        public bool IsShowEdit { get; set; }
        [NoColumnMap]
        public bool IsShowDelete { get; set; }
        [NoColumnMap]
        public bool IsShowView { get; set; }
    }
}
