using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Notification
{
    public class EMS_EmailTemplate : BaseEntity
    {
        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string TemplateCode { get; set; }

        public string TemplateSubject { get; set; }

        public string TemplateBody { get; set; }

        [AuditLog(Label = "Active")]
        public bool IsActive { get; set; }        
    }
}
