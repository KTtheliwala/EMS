using EasyNetQ;

namespace TheTecniQ.Services.Messaging.Model
{
    [Queue("SendMailQueue", ExchangeName = "SendMailExchange")]
    public class SendMailMessage
    {
        public int MailQueueId { get; set; }
        public string Response { get; set; }
    }
}
