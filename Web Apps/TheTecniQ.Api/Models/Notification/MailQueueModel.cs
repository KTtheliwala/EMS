using System;
using FluentValidation;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Logging;

using TheTecniQ.Core.Domain.Notification;

namespace TheTecniQ.API.Models.Notification
{
    public class MailQueueModel : BaseModel
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

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int Status { get; set; } 

        public int Retry { get; set; } = 0;

        public DateTime? LastSendingTry { get; set; }

        public string LastResponse { get; set; }
    }
}
