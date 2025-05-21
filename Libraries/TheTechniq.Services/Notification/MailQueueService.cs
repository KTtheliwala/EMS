using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using TheTecniQ.Core.Configuration;
using TheTecniQ.Core.Domain.Messaging;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Data;
using TheTecniQ.Services.Common;
using TheTecniQ.Services.Logging;
using TheTecniQ.Services.Messaging;

namespace TheTecniQ.Services.Notification
{
    public partial class MailQueueService(IRepository<EMS_MailQueue> mailQueueRepository) : CommonService<EMS_MailQueue>(mailQueueRepository), IMailQueueService
    {
        private readonly IRepository<EMS_MailQueue> _mailQueueRepository = mailQueueRepository;
        #region Methods
        public virtual async Task<IList<EMS_MailQueue>> GetDataForSendMail()
        {
            return await _mailQueueRepository.QueryProcAsync<EMS_MailQueue>("GET_EMAIL_NOTIFICATION_DATA", null);
        }
        /// <summary>
        /// Add record into mail queue and based on config it send to messaging queue or handle in schedule task.
        /// </summary>
        /// <param name="mailQueue"></param>
        /// <returns></returns>
        public virtual async Task<EMS_MailQueue> InsertAsync(EMS_MailQueue mailQueue)
        {
            if (!System.Convert.ToBoolean(AppConfig.RabbitMQ.EnableSendMail))
            {
                mailQueue.Status = (int)EnumStatus.Pending;
                return await _mailQueueRepository.InsertAsync(mailQueue,0,"");
            }
            else
            {
                mailQueue.Status = (int)EnumStatus.InProgress;
                var res = await _mailQueueRepository.InsertAsync(mailQueue,0,"");

                //var _messagingService = ServiceResolver.Resolve<IMessagingService>(); //To prevent DI issue since IMessagingService already use IMailQueueService

                //await _messagingService.EnqueueSendMailCreatedAsync(new SendMailQueueRequest
                //{
                //    MailQueueId = res.Id
                //});

                return res;
            }
        }
        #endregion
    }
}
