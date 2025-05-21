using System;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Infrastructure;

namespace TheTecniQ.Core.Domain.Logging
{
    /// <summary>
    /// Represents a log record
    /// </summary>
    public partial class Logs : BaseEntity
    {
        /// <summary>
        /// Gets or sets the log level identifier
        /// </summary>
        public int? ActionId { get; set; }


        /// <summary>
        /// Gets or sets the entity
        /// </summary>
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Message { get; set; }
        public string Level { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }

        /// <summary>
        /// Gets or sets the log level identifier
        /// </summary>
        public int? LogTypeId { get; set; }

        /// <summary>
        /// Gets or sets the log level identifier
        /// </summary>
        public int? LogSourceId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime? TimeStamp { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; }
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the action
        /// </summary>
        [NoColumnMap]
        public string Action
        {
            get
            {
                if (ActionId != null)
                    return ((EnumLogAction)ActionId).ToDescription();
                else
                    return null;
            }
        }



        /// <summary>
        /// Gets or sets the log type
        /// </summary>
        [NoColumnMap]
        public string LogType
        {
            get
            {
                if (LogTypeId != null)
                    return ((EnumLogType)LogTypeId).ToDescription();
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets or sets the log source
        /// </summary>
        [NoColumnMap]
        public string LogSource
        {
            get
            {
                if (LogSourceId != null)
                    return ((EnumLogSource)LogSourceId).ToDescription();
                else
                    return null;
            }
        }
    }
    public class LogReffResult : BaseEntity
    {
        public string Name { get; set; }
    }
}
