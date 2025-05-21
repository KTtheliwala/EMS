using EasyNetQ;

namespace TheTecniQ.Services.Messaging.Model
{
    [Queue("OrderCreatedQueue", ExchangeName = "OrderCreatedExchange")]
    public class OrderCreatedMessage
    {
        public int OrderId { get; set; }
    }
}
