using System;
using System.Collections.Generic;
using System.Text;

namespace TheTecniQ.Core.Domain.Messaging
{
    public class OrderQueueRequest : CommonQueueRequest
    {
        public int OrderId { get; set; }
    }

    public class OrderQueueResponse : CommonQueueResponse
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
    }
}
