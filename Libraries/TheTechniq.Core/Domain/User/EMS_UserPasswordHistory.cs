using System;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.User
{
    public class EMS_UserPasswordHistory : BaseEntity
    {
        public int UserId { get; set; }

        [AuditLog(Ignore = true)]
        public string Password { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
