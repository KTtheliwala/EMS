using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Core;
using System;
using TheTecniQ.API.Models.Common;

namespace TheTecniQ.Api.Models.AuditLog
{
    public partial class LogModel : BaseModel
    {
        public int? ActionId { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
        public int? LogTypeId { get; set; }
        public int? LogSourceId { get; set; }
        public DateTime? TimeStamp { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; }
        public string Username { get; set; }
        [NoColumnMap]
        public EnumLogAction Action
        {
            get => (EnumLogAction)(ActionId ?? 0);
            set => ActionId = (int)value;
        }
        [NoColumnMap]
        public EnumLogType LogType
        {
            get => (EnumLogType)(LogTypeId ?? 0);
            set => LogTypeId = (int)value;
        }

        [NoColumnMap]
        public EnumLogSource LogSource
        {
            get => (EnumLogSource)(LogSourceId ?? 0);
            set => LogSourceId = (int)value;
        }
    }
    public class LogReffResult : BaseEntity
    {
        public string Name { get; set; }
    }
}
