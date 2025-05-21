namespace TheTecniQ.Core.Configuration
{
    public partial class AppConfig
    {
        public static string Environment { get; set; }
        public static Authentication Authentication { get; set; }
        public static AESKeys AESKeys { get; set; }
        public static string CorsAllowUrls { get; set; }
        public static string SwaggerUrl { get; set; }
        public static string AllowRequestLog { get; set; }
        public static Logging Logging { get; set; }
        public static RabbitMQ RabbitMQ { get; set; }
        public static TwoFactor TwoFactor { get; set; }
        public static string FileUploadBasePath { get; set; }
    }
    public class Authentication
    {
        public string SecretKey { get; set; }
        public string APISecretKey { get; set; }
        public string APIClientId { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
    public class AESKeys
    {
        public string Key { get; set; }
        public string IV { get; set; }
    }
    public class Logging
    {
        public string BatchSize { get; set; }
        public string PeriodSecond { get; set; }
    }
    public class LoggingSettings
    {
        public int BatchSize { get; set; } = 100;
        public double PeriodSecond { get; set; } = 5;
        public bool SaveRequestResponse { get; set; } = false;
    }
    public class RabbitMQ
    {
        public string ConnectionString { get; set; }
        public string EnableSendMail { get; set; }
    }
    public class TwoFactor
    {
        public string IsEnable { get; set; }
        public string AppName { get; set; }
        public string TitleForamt { get; set; }
        public string SecretKeyFormat { get; set; }
    }
}