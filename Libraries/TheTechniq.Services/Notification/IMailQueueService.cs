using System.Collections.Generic;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Services.Common;

namespace TheTecniQ.Services.Notification
{
    public interface IMailQueueService:ICommonService<EMS_MailQueue>
    {
        Task<IList<EMS_MailQueue>> GetDataForSendMail();
        new Task<EMS_MailQueue> InsertAsync(EMS_MailQueue mailQueue);
    }
}