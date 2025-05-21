using System;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.User
{
    public class EMS_User : BaseEntity, ISoftDeletedEntity
    {

        [AuditLog(Label = "Role Name", ReferenceTable = nameof(EMS_Role), ReferenceColumn = nameof(EMS_Role.Name))]
        public int RoleId { get; set; }

        public string UserName { get; set; }

        [AuditLog(Ignore = true)]
        public string Password { get; set; }

        [AuditLog(Label = "First Name")]
        public string FirstName { get; set; }

        [AuditLog(Label = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        [AuditLog(Ignore = true)]
        public string UserToken { get; set; }

        [AuditLog(Label = "Active")]
        public bool IsActive { get; set; } = true;

        [AuditLog(Ignore = true)]
        public bool IsDeleted { get; set; } = false;
        public DateTime? LastLoggedInOn { get; set; }
        public DateTime? PasswordChangedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        [AuditLog(Ignore = true)]
        public bool Is2FA { get; set; }
        [AuditLog(Ignore = true)]
        public string TwoFaSecretKey { get; set; }

        [NoColumnMap]
        public string RoleName { get; set; }
        [NoColumnMap]
        public string FullName { get; set; }

    }
}
