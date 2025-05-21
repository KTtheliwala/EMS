using System;
using System.Threading.Tasks;
using EasyNetQ;

using TheTecniQ.Core.Configuration;
using TheTecniQ.Core.Domain.Messaging;
using TheTecniQ.Services.Messaging.Model;
using TheTecniQ.Services.Notification;

namespace TheTecniQ.Services.Messaging
{
    public class MessagingService(IMailQueueService mailQueueService) : IMessagingService
    {
        private readonly IMailQueueService _mailQueueService = mailQueueService;


        #region Common

        /// <summary>
        /// De-Queue all broker. Method to use when all broker handle by same server/host. This will reduce CreateBus (Connection) operation.
        /// Opening a connection for every operation (e.g. publishing a message) would be very inefficient and is highly discouraged.
        /// </summary>
        /// <returns></returns>
        public async Task DequeueAllAsync()
        {
            var bus = RabbitHutch.CreateBus(AppConfig.RabbitMQ.ConnectionString);
            await Task.Run(async () =>
            {
                await bus.PubSub.SubscribeAsync<OrderCreatedMessage>(string.Empty,
                async message => await Task.Factory.StartNew(() =>
                {
                    Console.WriteLine(string.Format("WorkerService: StartNew Dispatched Queue for OrderId: {0}", message.OrderId)); //TODO

                }).ContinueWith(task =>
                {
                    if (task.IsCompleted && !task.IsFaulted)
                    {
                        Console.WriteLine(string.Format("WorkerService: Completed Dispatched Queue for OrderId: {0}", message.OrderId));
                    }
                    else
                    {
                        Console.WriteLine("WorkerService: Message processing exception. Check the default error queue (broker)");
                    }
                }));

                await bus.PubSub.SubscribeAsync<SendMailMessage>(
                    string.Empty, async msg =>
                    {
                        Console.WriteLine(string.Format("WorkerService: StartNew Queue for MailQueueId: {0}", msg.MailQueueId));

                        await _mailQueueService.GetByIdAsync(msg.MailQueueId);
                    }
                );
            });
        }

        #endregion

        #region Order

        /// <summary>
        /// Enqueue Order to message broker
        /// </summary>
        /// <param name="orderQueueResponse"></param>
        /// <returns></returns>
        public async Task EnqueueOrderCreatedAsync(OrderQueueRequest orderQueueRequest)
        {
            using (var bus = RabbitHutch.CreateBus(AppConfig.RabbitMQ.ConnectionString))
            {
                await bus.PubSub.PublishAsync(new OrderCreatedMessage { OrderId = orderQueueRequest.OrderId });
            };
        }

        /// <summary>
        /// De-Queue Order from message broker
        /// </summary>
        /// <returns></returns>
        public async Task DequeueOrderCreatedAsync()
        {
            var bus = RabbitHutch.CreateBus(AppConfig.RabbitMQ.ConnectionString);
            await Task.Run(async () =>
            {
                await bus.PubSub.SubscribeAsync<OrderCreatedMessage>(
                    string.Empty, msg =>
                    {
                        Console.WriteLine(string.Format("WorkerService: StartNew Dispatched Queue for OrderId: {0}", msg.OrderId)); //TODO
                    }
                );
            });
        }

        #endregion

        #region Send mail

        /// <summary>
        /// Enqueue send mail to message broker
        /// </summary>
        /// <param name="sendMailQueueResponse"></param>
        /// <returns></returns>
        public async Task EnqueueSendMailCreatedAsync(SendMailQueueRequest sendMailQueueRequest)
        {
            using (var bus = RabbitHutch.CreateBus(AppConfig.RabbitMQ.ConnectionString))
            {
                await bus.PubSub.PublishAsync(new SendMailMessage { MailQueueId = sendMailQueueRequest.MailQueueId });
            };
        }

        /// <summary>
        /// De-Queue send mail from message broker
        /// </summary>
        /// <returns></returns>
        public async Task DequeueSendMailCreatedAsync()
        {
            var bus = RabbitHutch.CreateBus(AppConfig.RabbitMQ.ConnectionString);
            await Task.Run(async () =>
            {
                await bus.PubSub.SubscribeAsync<SendMailMessage>(
                    string.Empty, async msg =>
                    {
                        Console.WriteLine(string.Format("WorkerService: StartNew Queue for MailQueueId: {0}", msg.MailQueueId));

                        await _mailQueueService.GetByIdAsync(msg.MailQueueId);
                    }
                );
            });
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
