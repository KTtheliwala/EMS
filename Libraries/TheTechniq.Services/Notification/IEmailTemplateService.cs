using System.Threading.Tasks;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Services.Common;

namespace TheTecniQ.Services.Notification
{
    public partial interface IEmailTemplateService: ICommonService<EMS_EmailTemplate>
    {
        Task<EMS_EmailTemplate> GetByTagAsync(string Code);
    }
}