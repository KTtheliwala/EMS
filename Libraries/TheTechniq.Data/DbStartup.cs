using System;
using System.Linq;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TheTecniQ.Data;

/// <summary>
/// Represents object for the configuring DB context on application startup
/// </summary>
public static class DbStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //Register FluentMigrator
        services.AddFluentMigratorCore()
            //.AddScoped<IProcessorAccessor, ProcessorAccessor>()
            // set accessor for the connection string
            .AddScoped<IConnectionStringAccessor>(x => DataSettingsManager.LoadSettings())
            //.AddSingleton<IMigrationManager, MigrationManager>()
            //.AddSingleton<IConventionSet, ConventionSet>()
            .ConfigureRunner(rb =>
                rb.AddSqlServer() //.AddMySql5().AddPostgres()
                    // define the assembly containing the migrations
                    .ScanIn(typeof(DbStartup).Assembly).For.Migrations());

        services.AddTransient(p => new Lazy<IVersionLoader>(p.GetRequiredService<IVersionLoader>()));

        //data layer
        services.AddTransient<IDataProviderManager, DataProviderManager>();
        services.AddTransient(serviceProvider =>
            serviceProvider.GetRequiredService<IDataProviderManager>().DataProvider);

        //repositories	
        services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
    }

    /// <summary>
    /// Executes all found (and unapplied) migrations
    /// </summary>
    public static void ApplyUpMigrations(IServiceProvider services)
    {
        IFilteringMigrationSource _filteringMigrationSource = services.GetService<IFilteringMigrationSource>();
        IMigrationRunner _migrationRunner = services.GetService<IMigrationRunner>();
        IMigrationRunnerConventions _migrationRunnerConventions = services.GetService<IMigrationRunnerConventions>();

        System.Collections.Generic.IEnumerable<IMigration> migrations = _filteringMigrationSource.GetMigrations(null) ?? [];

        IOrderedEnumerable<FluentMigrator.Infrastructure.IMigrationInfo> sortedMigrations = migrations.Select(m => _migrationRunnerConventions.GetMigrationInfoForMigration(m)).OrderBy(migration => migration.Version);

        foreach (FluentMigrator.Infrastructure.IMigrationInfo migrationInfo in sortedMigrations)
        {
            _migrationRunner.MigrateUp(migrationInfo.Version);
        }
    }

    #region No Use

    /*/// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void Configure(IApplicationBuilder application)
    {
        CacheConfig cacheConfig = new();
        AppSettingsHelper.Config?.GetSection("CacheConfig").Bind(cacheConfig);

        LinqToDB.Common.Configuration.Linq.DisableQueryCache = cacheConfig.LinqDisableQueryCache;
    }
    
    Moved code from \Libraries\TheTecniQ.Core\Configuration\CacheConfig.cs
    namespace TheTecniQ.Core.Configuration;

    /// <summary>
    /// Represents cache configuration parameters
    /// </summary>
    public partial class CacheConfig
    {
        /// <summary>
        /// Gets or sets the default cache time in minutes
        /// </summary>
        public int DefaultCacheTime { get; protected set; } = 60;

        /// <summary>
        /// Gets or sets whether to disable linq2db query cache
        /// </summary>
        public bool LinqDisableQueryCache { get; protected set; } = false;
    }
    */

    #endregion
}