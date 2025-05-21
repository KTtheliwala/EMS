using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Logging;
using Serilog.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TheTecniQ.Services.Extensions
{
    public static class LoggingHelper
    {
        /// <summary>
        /// Sets Serilog as the logging provider.
        /// </summary>
        public static IHostBuilder UseSerilog(this IHostBuilder builder)
        {
            return builder.UseSerilog((context, services, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
                config.Enrich.FromLogContext();
            });
        }

        /// <summary>
        /// Initializes logger file.
        /// File name format is => {ApplicationName}.{LogRollingDate}.log
        /// </summary>
        public static void InitializeLogger(string[] args, WebApplicationBuilder builder)
        {
            // Build configuration for logger.
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            Serilog.Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(configRoot.GetSection("APISettings"))
             .Enrich.FromLogContext() // Add additional sinks or configurations here
             .CreateBootstrapLogger();
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });
            builder.Host.UseSerilog(Serilog.Log.Logger);
        }
    }
}
