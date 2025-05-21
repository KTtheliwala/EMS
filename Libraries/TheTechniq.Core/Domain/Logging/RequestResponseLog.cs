using System;

namespace TheTecniQ.Core.Domain.Logging
{
    public class RequestResponseLog : BaseEntity
    {
        public string Path { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public string ResponseCode { get; set; } = string.Empty;
        public DateTime RequestedOn { get; set; } = DateTime.UtcNow;
        public DateTime RespondedOn { get; set; } = DateTime.UtcNow;
        public bool IsSuccessStatusCode { get; set; } = true;
    }
}