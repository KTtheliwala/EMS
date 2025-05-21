using System;

using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Data.Configuration;
using TheTecniQ.Data.DataProviders;

namespace TheTecniQ.Data;

/// <summary>
/// Represents the data provider manager
/// </summary>
public partial class DataProviderManager : IDataProviderManager
{
    #region Methods

    /// <summary>
    /// Gets data provider by specific type
    /// </summary>
    /// <param name="dataProviderType">Data provider type</param>
    /// <returns></returns>
    public static IDatabaseProvider GetDataProvider(DataProviderType dataProviderType)
    {
        return dataProviderType switch
        {
            DataProviderType.SqlServer => new MsSqlDataProvider(),
            //DataProviderType.MySql => new MySqlDataProvider(),
            //DataProviderType.PostgreSQL => new PostgreSqlDataProvider(),
            _ => throw new Exception($"Not supported data provider name: '{dataProviderType}'"),
        };
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets data provider
    /// </summary>
    public IDatabaseProvider DataProvider
    {
        get
        {
            var dataProviderType = Singleton<DataConfig>.Instance.DataProvider;

            return GetDataProvider(dataProviderType);
        }
    }

    #endregion
}