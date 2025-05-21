using System;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.User
{
    public class EMS_Role : BaseEntity, ISoftDeletedEntity
    {
        public string Name { get; set; }

        [AuditLog(Label = "Active")]
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

    }
}
