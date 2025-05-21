using System;
using System.Collections.Generic;
using System.Text;

namespace TheTecniQ.Core.Domain.Messaging
{
    public class SendMailQueueRequest : CommonQueueRequest
    {
        public int MailQueueId { get; set; }
    }

    public class SendMailQueueResponse : CommonQueueResponse
    {
        public int MailQueueId { get; set; }
        public string Response { get; set; }
    }
}
