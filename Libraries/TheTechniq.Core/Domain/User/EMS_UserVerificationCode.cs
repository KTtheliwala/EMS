using System;

namespace TheTecniQ.Core.Domain.User
{
    public class EMS_UserVerificationCode : BaseEntity
    {
        public int UserId { get; set; }

        public string Code { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime ExpireDate { get; set; } = DateTime.UtcNow;

        public bool IsUse { get; set; } = false;
    }
}
