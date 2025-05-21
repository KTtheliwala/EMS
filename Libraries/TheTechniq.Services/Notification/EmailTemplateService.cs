using System.Linq;
using System.Threading.Tasks;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Core.Domain.User;

//using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Data;
using TheTecniQ.Services.Common;
//using TheTecniQ.Services.Logging;

namespace TheTecniQ.Services.Notification
{
    public partial class EmailTemplateService(IRepository<EMS_EmailTemplate> emailTemplateRepository) : CommonService<EMS_EmailTemplate>(emailTemplateRepository), IEmailTemplateService
    {
        #region Fields
        private readonly IRepository<EMS_EmailTemplate> _emailTemplateRepository = emailTemplateRepository;
        #endregion

        #region Methods
        public virtual async Task<EMS_EmailTemplate> GetByTagAsync(string Code)
        {
            var qry = from d in _emailTemplateRepository.Table
                      where d.TemplateCode.ToLower().TrimStart().TrimEnd() == Code.ToLower().TrimStart().TrimEnd()
                      select d;
            return await qry.FirstOrDefaultAsync();
        }
        #endregion
    }
}