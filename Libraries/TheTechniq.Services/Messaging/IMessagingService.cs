using System;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Messaging;

namespace TheTecniQ.Services.Messaging
{
    /// <summary>
    /// Interface for the Order Queue Repository
    /// </summary>
    public interface IMessagingService : IDisposable
    {
        #region Common

        /// <summary>
        /// De-Queue all broker. Method to use when all broker handle by same server/host. This will reduce CreateBus (Connection) operation.
        /// Opening a connection for every operation (e.g. publishing a message) would be very inefficient and is highly discouraged.
        /// </summary>
        /// <returns></returns>
        Task DequeueAllAsync();

        #endregion

        #region Order

        /// <summary>
        /// Enqueue Order to message broker
        /// </summary>
        /// <param name="orderQueueResponse"></param>
        /// <returns></returns>
        Task EnqueueOrderCreatedAsync(OrderQueueRequest orderQueueRequest);

        /// <summary>
        /// De-Queue Order from message broker
        /// </summary>
        /// <returns></returns>
        Task DequeueOrderCreatedAsync();

        #endregion

        #region Send mail

        /// <summary>
        /// Enqueue send mail to message broker
        /// </summary>
        /// <param name="sendMailQueueResponse"></param>
        /// <returns></returns>
        Task EnqueueSendMailCreatedAsync(SendMailQueueRequest sendMailQueueRequest);

        /// <summary>
        /// De-Queue send mail from message broker
        /// </summary>
        /// <returns></returns>
        Task DequeueSendMailCreatedAsync();

        #endregion
    }
}
