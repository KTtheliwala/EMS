using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Permissions
{
    public class EMS_Permission : BaseEntity
    {
        [AuditLog(Label = "Role", ReferenceTable = nameof(EMS_Role), ReferenceColumn = nameof(EMS_Role.Name))]
        public int RoleId { get; set; }

        [AuditLog(Label = "Page", ReferenceTable = nameof(EMS_Page), ReferenceColumn = nameof(EMS_Page.PageName))]
        public int PageId { get; set; }

        [AuditLog(Label = "View Permission")]
        public bool IsView { get; set; } = true;

        [AuditLog(Label = "Add Permission")]
        public bool IsAdd { get; set; } = true;

        [AuditLog(Label = "Edit Permission")]
        public bool IsEdit { get; set; } = true;

        [AuditLog(Label = "Delete Permission")]
        public bool IsDelete { get; set; } = true;
    }
}
