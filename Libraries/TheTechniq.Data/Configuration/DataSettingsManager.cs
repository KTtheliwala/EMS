using System;
using Microsoft.Extensions.Configuration;

using TheTecniQ.Core.Configuration;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Data.Configuration;

namespace TheTecniQ.Data;

/// <summary>
/// Represents the data settings manager
/// </summary>
public partial class DataSettingsManager
{
    #region Methods

    /// <summary>
    /// Load data settings
    /// </summary>
    /// <param name="fileProvider">File provider</param>
    /// <param name="reload">Force loading settings from disk</param>
    /// <returns>Data settings</returns>
    public static DataConfig LoadSettings(bool reload = false)
    {
        if (!reload && Singleton<DataConfig>.Instance is not null)
            return Singleton<DataConfig>.Instance;

        if (AppSettingsHelper.Config == null)
            throw new ArgumentNullException("DatabaseStrings", "DatabaseStrings not configured");

        DataConfig DBSettings = new();
        AppSettingsHelper.Config?.GetSection("DatabaseStrings").Bind(DBSettings);

        Singleton<DataConfig>.Instance = DBSettings;

        return Singleton<DataConfig>.Instance;
    }

    /// <summary>
    /// Gets the command execution timeout.
    /// </summary>
    /// <value>
    /// Number of seconds. Negative timeout value means that a default timeout will be used. 0 timeout value corresponds to infinite timeout.
    /// </value>
    public static int GetSqlCommandTimeout()
    {
        return LoadSettings()?.SQLCommandTimeout ?? -1;
    }

    /// <summary>
    /// Gets a value that indicates whether to add NoLock hint to SELECT statements (applies only to SQL Server, otherwise returns false)
    /// </summary>
    public static bool UseNoLock()
    {
        var settings = LoadSettings();

        if (settings is null)
            return false;

        return settings.DataProvider == DataProviderType.SqlServer && settings.WithNoLock;
    }

    #endregion
}