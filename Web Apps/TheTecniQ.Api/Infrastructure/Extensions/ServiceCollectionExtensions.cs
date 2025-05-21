using System;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using AutoMapper;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Events;

using TheTecniQ.Core.Configuration;
using TheTecniQ.Services.Extensions;
using TheTecniQ.Data;
using System.Linq;
using TheTecniQ.Services.Barcode;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace TheTecniQ.API.Infrastructure.Extensions;

/// <summary>
/// Represents extensions of IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure base application settings
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="builder">A builder for web applications and services</param>
    public static void ConfigureApplicationSettings(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        //Load appsettings.json
        AppSettingsHelper.Initialize(builder.Configuration);
        AppConfig appConfig = new();
        builder.Configuration.GetSection("APISettings").Bind(appConfig);
        //Load database settings
        DataSettingsManager.LoadSettings();

    }

    /// <summary>
    /// Add services to the application and configure service provider
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="builder">A builder for web applications and services</param>
    public static void ConfigureApplicationServices(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        //let the operating system decide what TLS protocol version to use
        //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
        ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

        //add accessor to HttpContext
        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();

        //Configure Serilog
       // services.AddLogging(builder);

        //Form configs
        services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = long.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
            options.Limits.MaxRequestBodySize = 104857600; // Max body size allow upto 100 MB 
            options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
            options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
        });
        services.Configure<IISServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
            options.MaxRequestBodySize = 104857600; // Set to 100MB (in bytes)
        });

        //JOSN object configs
        services.AddMvc(opt =>
        {
            opt.EnableEndpointRouting = false;
            opt.Filters.Add(typeof(ValidateModelStateAttribute));
        }).AddJsonOptions(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; //Exclude null properties from JSON
            jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        //CORS Policy
        string[] CorsAllowUrls = AppConfig.CorsAllowUrls.Split(',');
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "apicorspolicy",
                builder =>
                {
                    builder
                    .WithOrigins(CorsAllowUrls).SetIsOriginAllowed(origin => CorsAllowUrls.Contains(new Uri(origin).Host))//.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Content-Disposition");
                });
        });

        //API Model validation
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters().AddValidatorsFromAssemblyContaining<Program>();
        /*services.AddMvc(opt =>
        {
            opt.EnableEndpointRouting = false;
            opt.Filters.Add(typeof(ValidateModelStateAttribute));
        });*/

        //API Versioning
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });

        //JWT Token Authentication
        byte[] secretKey = Encoding.ASCII.GetBytes(AppConfig.Authentication.SecretKey);
        builder.Services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        //Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API.Project", Version = "v1" });
            c.MapType<DateTime>(() => new OpenApiSchema { Format = "dd/MMM/yyyy hh:mm tt", Type = "DateTime" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "JWT Authorization header using the Bearer scheme.",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.\r\n\r\nExample: \"12345abcdef\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()

                    }
                });
        });

        //Auto mapper
        MapperConfiguration mapperConfiguration = new(configure => configure.AddProfile<ApplicationMappingProfile>());
        mapperConfiguration.CreateMapper().InitializeMapper();

        //Database start up
        DbStartup.ConfigureServices(services, builder.Configuration);

        //Execute pending migration
        DbStartup.ApplyUpMigrations(builder.Services.BuildServiceProvider());

        //Application dependency registration
        DependencyRegistrar.Register(services);
    }

    /// <summary>
    /// Register HttpContextAccessor
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddHttpContextAccessor(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();        
    }

    /// <summary>
    /// Adds services required for distributed cache
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddDistributedCache(this IServiceCollection services)
    {
        DistributedCacheConfig distributedCacheConfig = new();
        AppSettingsHelper.Config?.GetSection("DistributedCacheConfig").Bind(distributedCacheConfig);

        if (!distributedCacheConfig.Enabled)
            return;

        switch (distributedCacheConfig.DistributedCacheType)
        {
            case DistributedCacheType.Memory:
                services.AddDistributedMemoryCache(); //services.AddMemoryCache();
                break;

                /*case DistributedCacheType.SqlServer:
                    services.AddDistributedSqlServerCache(options =>
                    {
                        options.ConnectionString = distributedCacheConfig.ConnectionString;
                        options.SchemaName = distributedCacheConfig.SchemaName;
                        options.TableName = distributedCacheConfig.TableName;
                    });
                    break;

                case DistributedCacheType.Redis:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = distributedCacheConfig.ConnectionString;
                        options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                    });
                    break;

                case DistributedCacheType.RedisSynchronizedMemory:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = distributedCacheConfig.ConnectionString;
                        options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                    });
                    break;*/
        }
    }

    /// <summary>
    /// Adds authentication service
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddAuthentication(this IServiceCollection services)
    {

    }

    public static void AddLogging(this IServiceCollection services, WebApplicationBuilder builder)
    {

        //Log.Logger = new LoggerConfiguration()
        //     .ReadFrom.Configuration(builder.Configuration.GetSection("APISettings"))
        //     .Enrich.FromLogContext() // Add additional sinks or configurations here
        //     .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
        builder.Host.UseSerilog(Log.Logger);

    }

    private static LoggerConfiguration WriteToSqlServerByLevel(this LoggerConfiguration loggerConfiguration, string connectionString)
    {
        var defaultTableName = "Logs";

        var mssqlServerSink = new MSSqlServerSink(connectionString,
            new MSSqlServerSinkOptions { TableName = defaultTableName, AutoCreateSqlTable = true });

       
            /*.WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
                .WriteTo.Sink(batchedSink)
                .WriteTo.MSSqlServer(connectionString,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "ErrorLogs", AutoCreateSqlTable = true }))

            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Warning)
                .WriteTo.Sink(batchedSink)
                .WriteTo.MSSqlServer(connectionString,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "WarningLogs", AutoCreateSqlTable = true }))*/

          
            //TODO: with below filter also it still add all event in Logs tables.
            //Meaning above entry in separate table added but same again added in Logs table too
            //May be if we need to manage separate then in Listing code we exclude and from Schedule Task we remove
            //If common table for all logs then need to remove filter
          
        //string value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //TODO : can set env wise log e.g Infomation may not need to set in Production

        return loggerConfiguration;
    }
}