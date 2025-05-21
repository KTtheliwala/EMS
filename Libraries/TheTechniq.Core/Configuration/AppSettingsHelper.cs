using TheTecniQ.Core.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace TheTecniQ.Core.Configuration
{
    public static class AppSettingsHelper
    {
        public static IConfiguration Config;

        public static void Initialize(IConfiguration Configuration)
        {
            Config = Configuration;
        }

        /// <summary>
        /// Common configuration of all server apps (API, Schedule task etc)
        /// </summary>
        /// <returns>App settings</returns>
        public static AppConfig AppConfig
        {
            get
            {
                if (Singleton<AppConfig>.Instance != null)
                {
                    return Singleton<AppConfig>.Instance;
                }
                return null;
            }
        }
    }
}
