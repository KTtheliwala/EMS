using System;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Notification
{
    public class EMS_MailQueue : BaseEntity
    {
        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string ToEmail { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string MailSubject { get; set; }

        public string MailBody { get; set; }

        public string AttachmentName { get; set; }

        public string AttachmentLink { get; set; }

        [AuditLog(Label = "Created Date", DateFormat = "dd/MM/yyyy HH:mm")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int Status { get; set; }

        public int Retry { get; set; } = 0;

        [AuditLog(Label = "Last Sending Try", DateFormat = "dd/MM/yyyy HH:mm")]
        public DateTime? LastSendingTry { get; set; }

        public string LastResponse { get; set; }
    }
}
