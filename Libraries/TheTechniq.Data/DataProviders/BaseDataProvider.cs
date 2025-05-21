using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Tools;

using TheTecniQ.Core;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Data.Mapping;
using System.Data;
using System.Formats.Tar;
using static LinqToDB.DataProvider.MySql.MySqlHints;

namespace TheTecniQ.Data.DataProviders;

public abstract partial class BaseDataProvider
{
    #region Utilities

    /// <summary>
    /// Gets an additional mapping schema
    /// </summary>
    private MappingSchema GetMappingSchema()
    {
        if (Singleton<MappingSchema>.Instance is null)
        {
            Singleton<MappingSchema>.Instance = new MappingSchema(ConfigurationName);
            Singleton<MappingSchema>.Instance.AddMetadataReader(new FluentMigratorMetadataReader());
        }

        return Singleton<MappingSchema>.Instance;

    }

    /// <summary>
    /// Returns mapped entity descriptor.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>Mapping descriptor</returns>
    public EntityDescriptor GetEntityDescriptor<TEntity>() where TEntity : BaseEntity
    {
        return GetMappingSchema()?.GetEntityDescriptor(typeof(TEntity));
    }

    /// <summary>
    /// Gets a connection to the database for a current data provider
    /// </summary>
    /// <param name="connectionString">Connection string</param>
    /// <returns>Connection to a database</returns>
    protected abstract DbConnection GetInternalDbConnection(string connectionString);

    /// <summary>
    /// Creates the database connection
    /// </summary>
    /// <param name="isLogDB">Log database table</param>
    protected virtual DataConnection CreateDataConnection(bool isLogDB = false)
    {
        return CreateDataConnection(LinqToDbDataProvider, isLogDB);
    }

    /// <summary>
    /// Creates database command instance using provided command text and parameters.
    /// </summary>
    /// <param name="sql">Command text</param>
    /// <param name="dataParameters">Command parameters</param>
    protected virtual CommandInfo CreateDbCommand(string sql, DataParameter[] dataParameters)
    {
        ArgumentNullException.ThrowIfNull(dataParameters);

        var dataConnection = CreateDataConnection(LinqToDbDataProvider);

        return new CommandInfo(dataConnection, sql, dataParameters);
    }

    /// <summary>
    /// Creates the database connection
    /// </summary>
    /// <param name="dataProvider">Data provider</param>
    /// <param name="isLogDB">Log database table</param>
    /// <returns>Database connection</returns>
    protected virtual DataConnection CreateDataConnection(IDataProvider dataProvider, bool isLogDB = false, bool isExternalDB = false)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);

        string connectionString = null;
        if (isLogDB)
            connectionString = GetLogConnectionString();
        if (isExternalDB)
            connectionString = GetExternalConnectionString();

        var dataConnection = new DataConnection(dataProvider, CreateDbConnection(connectionString), GetMappingSchema())
        {
            CommandTimeout = DataSettingsManager.GetSqlCommandTimeout()
        };

        return dataConnection;
    }

    /// <summary>
    /// Creates a connection to a database
    /// </summary>
    /// <param name="connectionString">Connection string</param>
    /// <returns>Connection to a database</returns>
    protected virtual DbConnection CreateDbConnection(string connectionString = null)
    {
        return GetInternalDbConnection(!string.IsNullOrEmpty(connectionString) ? connectionString : GetCurrentConnectionString());
    }

    /// <summary>
    /// Gets a data hash from database side
    /// </summary>
    /// <param name="binaryData">Array for a hashing function</param>
    /// <returns>Data hash</returns>
    /// <remarks>
    /// For SQL Server 2014 (12.x) and earlier, allowed input values are limited to 8000 bytes.
    /// https://docs.microsoft.com/en-us/sql/t-sql/functions/hashbytes-transact-sql
    /// </remarks>
    [Sql.Expression("CONVERT(VARCHAR(128), HASHBYTES('SHA2_512', SUBSTRING({0}, 0, 8000)), 2)", ServerSideOnly = true, Configuration = ProviderName.SqlServer)]
    [Sql.Expression("SHA2({0}, 512)", ServerSideOnly = true, Configuration = ProviderName.MySql)]
    [Sql.Expression("encode(digest({0}, 'sha512'), 'hex')", ServerSideOnly = true, Configuration = ProviderName.PostgreSQL)]
    protected static string SqlSha2(object binaryData)
    {
        throw new InvalidOperationException("This function should be used only in database code");
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get hash values of a stored entity field
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="keySelector">A key selector which should project to a dictionary key</param>
    /// <param name="fieldSelector">A field selector to apply a transform to a hash value</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>Dictionary</returns>
    public virtual async Task<IDictionary<int, string>> GetFieldHashesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int>> keySelector,
        Expression<Func<TEntity, object>> fieldSelector) where TEntity : BaseEntity
    {
        if (keySelector.Body is not MemberExpression keyMember ||
            keyMember.Member is not PropertyInfo keyPropInfo)
        {
            throw new ArgumentException($"Expression '{keySelector}' refers to method or field, not a property.");
        }

        if (fieldSelector.Body is not MemberExpression member ||
            member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException($"Expression '{fieldSelector}' refers to a method or field, not a property.");
        }

        var hashes = GetTable<TEntity>()
            .Where(predicate)
            .Select(x => new
            {
                Id = Sql.Property<int>(x, keyPropInfo.Name),
                Hash = SqlSha2(Sql.Property<object>(x, propInfo.Name))
            });

        return await AsyncIQueryableExtensions.ToDictionaryAsync(hashes, p => p.Id, p => p.Hash);
    }

    /// <summary>
    /// Returns queryable source for specified mapping class for current connection,
    /// mapped to database table or view.
    /// </summary>
    /// <param name="isLogDB">Log database table</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>Queryable source</returns>
    public virtual IQueryable<TEntity> GetTable<TEntity>(bool isLogDB = false) where TEntity : BaseEntity
    {
        var options = new DataOptions()
            .UseConnectionString(LinqToDbDataProvider, isLogDB ? GetLogConnectionString() : GetCurrentConnectionString())
            .UseMappingSchema(GetMappingSchema());

        return new DataContext(options)
        {
            CommandTimeout = DataSettingsManager.GetSqlCommandTimeout()
        }
        .GetTable<TEntity>();
    }

    /// <summary>
    /// Inserts record into table. Returns inserted entity with identity
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the inserted entity
    /// </returns>
    public virtual async Task<TEntity> InsertEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        entity.Id = await dataContext.InsertWithInt32IdentityAsync(entity);
        return entity;
    }

    /// <summary>
    /// Inserts record into table. Returns inserted entity with identity
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns>Inserted entity</returns>
    public virtual TEntity InsertEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        entity.Id = dataContext.InsertWithInt32Identity(entity);
        return entity;
    }

    /// <summary>
    /// Updates record in table, using values from entity parameter.
    /// Record to update identified by match on primary key value from obj value.
    /// </summary>
    /// <param name="entity">Entity with data to update</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        await dataContext.UpdateAsync(entity);
    }

    /// <summary>
    /// Updates record in table, using values from entity parameter.
    /// Record to update identified by match on primary key value from obj value.
    /// </summary>
    /// <param name="entity">Entity with data to update</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public virtual void UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        dataContext.Update(entity);
    }

    /// <summary>
    /// Updates records in table, using values from entity parameter.
    /// Records to update are identified by match on primary key value from obj value.
    /// </summary>
    /// <param name="entities">Entities with data to update</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        //we don't use the Merge API on this level, because this API not support all databases.
        //you may see all supported databases by the following link: https://linq2db.github.io/articles/sql/merge/Merge-API.html#supported-databases
        foreach (var entity in entities)
            await UpdateEntityAsync(entity);
    }

    /// <summary>
    /// Updates records in table, using values from entity parameter.
    /// Records to update are identified by match on primary key value from obj value.
    /// </summary>
    /// <param name="entities">Entities with data to update</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public virtual void UpdateEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        //we don't use the Merge API on this level, because this API not support all databases.
        //you may see all supported databases by the following link: https://linq2db.github.io/articles/sql/merge/Merge-API.html#supported-databases
        foreach (var entity in entities)
            UpdateEntity(entity);
    }

    /// <summary>
    /// Deletes record in table. Record to delete identified
    /// by match on primary key value from obj value.
    /// </summary>
    /// <param name="entity">Entity for delete operation</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        await dataContext.DeleteAsync(entity);
    }

    /// <summary>
    /// Deletes record in table. Record to delete identified
    /// by match on primary key value from obj value.
    /// </summary>
    /// <param name="entity">Entity for delete operation</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public virtual void DeleteEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        dataContext.Delete(entity);
    }

    /// <summary>
    /// Performs delete records in a table
    /// </summary>
    /// <param name="entities">Entities for delete operation</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        if (entities.All(entity => entity.Id == 0))
        {
            foreach (var entity in entities)
                await dataContext.DeleteAsync(entity);
        }
        else
        {
            await dataContext.GetTable<TEntity>()
                .Where(e => e.Id.In(entities.Select(x => x.Id)))
                .DeleteAsync();
        }
    }

    /// <summary>
    /// Performs delete records in a table
    /// </summary>
    /// <param name="entities">Entities for delete operation</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public virtual void BulkDeleteEntities<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        if (entities.All(entity => entity.Id == 0))
            foreach (var entity in entities)
                dataContext.Delete(entity);
        else
            dataContext.GetTable<TEntity>()
                .Where(e => e.Id.In(entities.Select(x => x.Id)))
                .Delete();
    }

    /// <summary>
    /// Performs delete records in a table by a condition
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of deleted records
    /// </returns>
    public virtual async Task<int> BulkDeleteEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        return await dataContext.GetTable<TEntity>()
            .Where(predicate)
            .DeleteAsync();
    }

    /// <summary>
    /// Performs delete records in a table by a condition
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>
    /// The number of deleted records
    /// </returns>
    public virtual int BulkDeleteEntities<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection();
        return dataContext.GetTable<TEntity>()
            .Where(predicate)
            .Delete();
    }

    /// <summary>
    /// Performs bulk insert operation for entity collection.
    /// </summary>
    /// <param name="entities">Entities for insert operation</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task BulkInsertEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection(LinqToDbDataProvider);
        await dataContext.BulkCopyAsync(new BulkCopyOptions(), entities.RetrieveIdentity(dataContext));
    }

    /// <summary>
    /// Performs bulk insert operation for entity collection.
    /// </summary>
    /// <param name="entities">Entities for insert operation</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public virtual void BulkInsertEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection(LinqToDbDataProvider);
        dataContext.BulkCopy(new BulkCopyOptions(), entities.RetrieveIdentity(dataContext));
    }

    /// <summary>
    /// Executes command asynchronously and returns number of affected records
    /// </summary>
    /// <param name="sql">Command text</param>
    /// <param name="dataParameters">Command parameters</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of records, affected by command execution.
    /// </returns>
    public virtual async Task<int> ExecuteNonQueryAsync(string sql, params DataParameter[] dataParameters)
    {
        var command = CreateDbCommand(sql, dataParameters);

        return await command.ExecuteAsync();
    }

    /// <summary>
    /// Executes command asynchronously and returns single entity
    /// </summary>
    /// <param name="sql">Command text</param>
    /// <param name="parameters">Command parameters</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the value of specified type
    /// </returns>
    public virtual async Task<T> ExecuteNonQueryAsync<T>(string sql, params DataParameter[] dataParameters)
    {
        var command = CreateDbCommand(sql, dataParameters);

        return await command.ExecuteAsync<T>();
    }

    /// <summary>
    /// Executes command using System.Data.CommandType.StoredProcedure command type and
    /// returns results as collection of values of specified type
    /// </summary>
    /// <typeparam name="T">Result record type</typeparam>
    /// <param name="procedureName">Procedure name</param>
    /// <param name="parameters">Command parameters</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the returns collection of query result records
    /// </returns>
    public virtual async Task<IList<T>> QueryProcAsync<T>(string procedureName, params DataParameter[] dataParameters)
    {
        var command = CreateDbCommand(procedureName, dataParameters);
        var rez = await command.QueryProcAsync<T>();
        return rez.ToList();
    }

    /// <summary>
    /// Executes command using System.Data.CommandType.StoredProcedure command type and
    /// returns result as values of specified type
    /// </summary>
    /// <typeparam name="T">Result record type</typeparam>
    /// <param name="procedureName">Procedure name</param>
    /// <param name="parameters">Command parameters</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the returns of query result record
    /// </returns>
    public virtual async Task<T> QueryProcEntityAsync<T>(string procedureName, params DataParameter[] dataParameters)
    {
        var command = CreateDbCommand(procedureName, dataParameters);
        var rez = await command.QueryProcAsync<T>();
        return rez.FirstOrDefault();
    }

    /// <summary>
    /// Executes SQL command and returns results as collection of values of specified type
    /// </summary>
    /// <typeparam name="T">Type of result items</typeparam>
    /// <param name="sql">SQL command text</param>
    /// <param name="parameters">Parameters to execute the SQL command</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the collection of values of specified type
    /// </returns>
    public virtual Task<IList<T>> QueryAsync<T>(string sql, params DataParameter[] dataParameters)
    {
        using var dataContext = CreateDataConnection();
        return Task.FromResult<IList<T>>(dataContext.Query<T>(sql, dataParameters)?.ToList() ?? []);
    }

    /// <summary>
    /// Executes SQL command and returns result of values of specified type
    /// </summary>
    /// <typeparam name="T">Type of result items</typeparam>
    /// <param name="sql">SQL command text</param>
    /// <param name="parameters">Parameters to execute the SQL command</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the values of specified type
    /// </returns>
    public virtual Task<T> QueryEntityAsync<T>(string sql, params DataParameter[] dataParameters)
    {
        using var dataContext = CreateDataConnection();
        return Task.FromResult<T>(dataContext.Query<T>(sql, dataParameters).FirstOrDefault());
    }

    

    /// <summary>
    /// Truncates database table
    /// </summary>
    /// <param name="resetIdentity">Performs reset identity column</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public virtual async Task TruncateAsync<TEntity>(bool resetIdentity = false) where TEntity : BaseEntity
    {
        using var dataContext = CreateDataConnection(LinqToDbDataProvider);
        await dataContext.GetTable<TEntity>().TruncateAsync(resetIdentity);
    }

    public virtual async Task<QueryMultiResult<T1, T2>> QueryProcMultipleAsync<T1, T2>(string procedureName, params DataParameter[] dataParameters)
    {
        var command = CreateDbCommand(procedureName, dataParameters);

        var results = await command.QueryProcMultipleAsync<QueryMultiResult<T1, T2>>();

        return results;
    }
    //public async Task<DataSet> ExecuteSqlQueryForDataSetAsync(string sqlQuery, params DataParameter[] dataParameters)
    //{
    //    using var dataContext = CreateDataConnection();
    //    dataContext.CommandTimeout = 200;
    //    DataSet ds = new();

    //    using (var dataReader = await ExecuteReaderAsyncWrapper(dataContext, sqlQuery, CommandType.Text, CommandBehavior.Default, dataParameters))
    //    {
    //        do
    //        {
    //            if (await dataReader.Reader.ReadAsync())
    //            {
    //                var dataTable = new DataTable();
    //                string[] fieldNames = Enumerable.Range(0, dataReader.Reader.FieldCount).Select(i => dataReader.Reader.GetName(i)).ToArray();
    //                foreach (string field in fieldNames)
    //                {
    //                    dataTable.Columns.Add(field);
    //                }

    //                do
    //                {
    //                    DataRow dr = dataTable.NewRow();
    //                    for (int index = 0; index < fieldNames.Length; index++)
    //                    {
    //                        dr[fieldNames[index]] = dataReader.Reader.GetValue(index);
    //                    }
    //                    dataTable.Rows.Add(dr);
    //                } while (await dataReader.Reader.ReadAsync());
    //                ds.Tables.Add(dataTable);
    //            }

    //        } while (await dataReader.Reader.NextResultAsync());
    //    }
    //    return ds;
    //}
    public async Task<DataSet> ExecuteStoredProcedureForDataSetAsync(string procedureName, CommandType cmdType = CommandType.StoredProcedure, bool isLogDB = false, params DataParameter[] dataParameters)
    {
        using var dataContext = CreateDataConnection(isLogDB);
        dataContext.CommandTimeout = 200;
        DataSet ds = new();

        using (var dataReader = await ExecuteReaderAsyncWrapper(dataContext, procedureName, cmdType, CommandBehavior.Default, dataParameters))
        {
            do
            {
                if (await dataReader.Reader.ReadAsync())
                {
                    var dataTable = new DataTable();
                    string[] fieldNames = Enumerable.Range(0, dataReader.Reader.FieldCount).Select(i => dataReader.Reader.GetName(i)).ToArray();
                    foreach (string field in fieldNames)
                    {
                        dataTable.Columns.Add(field);
                    }

                    do
                    {
                        DataRow dr = dataTable.NewRow();
                        for (int index = 0; index < fieldNames.Length; index++)
                        {
                            dr[fieldNames[index]] = dataReader.Reader.GetValue(index);
                        }
                        dataTable.Rows.Add(dr);
                    } while (await dataReader.Reader.ReadAsync());
                    ds.Tables.Add(dataTable);
                }

            } while (await dataReader.Reader.NextResultAsync());
        }
        return ds;
    }
    public async Task<int> ExecuteStoredProcedureForInsertedIdAsync(
    string procedureName,
    CommandType cmdType = CommandType.StoredProcedure,
    bool isLogDB = false,
    string returnParamName= "Inserted",
    params DataParameter[] dataParameters)
    {
        using var dataContext = CreateDataConnection(isLogDB);
        dataContext.CommandTimeout = 200;

        // Define the output parameter for @Inserted
        var insertedParameter = new DataParameter
        {
            Name = "@"+ returnParamName,
            DataType = LinqToDB.DataType.Int32,
            Direction = ParameterDirection.Output,
            Value = DBNull.Value
        };

        // Combine input parameters with the output parameter
        var allParameters = dataParameters.Concat(new[] { insertedParameter }).ToArray();

        // Execute the stored procedure
        await dataContext.ExecuteProcAsync(procedureName, allParameters);

        // Retrieve the output parameter value after execution
        return insertedParameter.Value != DBNull.Value ? Convert.ToInt32(insertedParameter.Value) : 0;
    }

    public async Task ExecuteStoredForUpdateProcedureAsync(
    string procedureName,
    CommandType cmdType = CommandType.StoredProcedure,
    bool isLogDB = false,
    params DataParameter[] dataParameters)
    {
        using var dataContext = CreateDataConnection(isLogDB);
        dataContext.CommandTimeout = 200;

        // Execute the stored procedure
        await dataContext.ExecuteProcAsync(procedureName, dataParameters);
    }

    protected virtual Task<DataReader> ExecuteReaderAsyncWrapper(DataConnection dataConnection, string procedureName, CommandType commandType, CommandBehavior commandBehavior, params DataParameter[] parameters)
    {
        return Task.Run(() =>
        {
            return dataConnection.ExecuteReader(procedureName, commandType, commandBehavior, parameters);
        });
    }

    public async Task<DataSet> ExecuteStoredProcedureForDataSetAsync(string procedureName, bool isLogDB = false, params DataParameter[] dataParameters)
    {
        using var dataContext = CreateDataConnection(isLogDB);
        dataContext.CommandTimeout = 200;
        DataSet ds = new();

        using (var dataReader = await ExecuteReaderAsyncWrapper(dataContext, procedureName, CommandType.StoredProcedure, CommandBehavior.Default, dataParameters))
        {
            do
            {
                if (dataReader.Reader.Read())
                {
                    var dataTable = new DataTable();
                    string[] fieldNames = Enumerable.Range(0, dataReader.Reader.FieldCount).Select(i => dataReader.Reader.GetName(i)).ToArray();
                    foreach (string field in fieldNames)
                    {
                        dataTable.Columns.Add(field);
                    }

                    do
                    {
                        DataRow dr = dataTable.NewRow();
                        for (int index = 0; index < fieldNames.Length; index++)
                        {
                            dr[fieldNames[index]] = dataReader.Reader.GetValue(index);
                        }
                        dataTable.Rows.Add(dr);
                    } while (dataReader.Reader.Read());
                    ds.Tables.Add(dataTable);
                }

            } while (dataReader.Reader.NextResult());
        }
        return ds;
    }

    public async Task<DataSet> ExecuteStoredProcedureForDataSetAsync22(string procedureName, bool isLogDB = false, params DataParameter[] dataParameters)
    {
        using var dataContext = CreateDataConnection();
        dataContext.CommandTimeout = 200;
        DataSet ds = new();

        using (var dataReader = await ExecuteReaderAsyncWrapper(dataContext, procedureName, CommandType.Text, CommandBehavior.Default, dataParameters))
        {
            do
            {
                if (await dataReader.Reader.ReadAsync())
                {
                    var dataTable = new DataTable();
                    string[] fieldNames = Enumerable.Range(0, dataReader.Reader.FieldCount).Select(i => dataReader.Reader.GetName(i)).ToArray();
                    foreach (string field in fieldNames)
                    {
                        dataTable.Columns.Add(field);
                    }

                    do
                    {
                        DataRow dr = dataTable.NewRow();
                        for (int index = 0; index < fieldNames.Length; index++)
                        {
                            dr[fieldNames[index]] = dataReader.Reader.GetValue(index);
                        }
                        dataTable.Rows.Add(dr);
                    } while (await dataReader.Reader.ReadAsync());
                    ds.Tables.Add(dataTable);
                }

            } while (await dataReader.Reader.NextResultAsync());
        }
        return ds;
    }

    public virtual DataSet ExecuteStoredProcedureForDataSet(string procedureName, params DataParameter[] dataParameters)
    {
        using DataConnection dataContext = CreateDataConnection();
        dataContext.CommandTimeout = 200;
        System.Data.DataSet ds = new();

        using (DataReader dataReader = dataContext.ExecuteReader(procedureName, System.Data.CommandType.StoredProcedure, System.Data.CommandBehavior.Default, dataParameters))
        {
            do
            {
                if (dataReader.Reader.Read())
                {
                    var dataTable = new DataTable();
                    string[] fieldNames = Enumerable.Range(0, dataReader.Reader.FieldCount).Select(i => dataReader.Reader.GetName(i)).ToArray();
                    foreach (string field in fieldNames)
                    {
                        dataTable.Columns.Add(field);
                    }

                    do
                    {
                        DataRow dr = dataTable.NewRow();
                        for (int index = 0; index < fieldNames.Length; index++)
                        {
                            dr[fieldNames[index]] = dataReader.Reader.GetValue(index);
                        }
                        dataTable.Rows.Add(dr);
                    } while (dataReader.Reader.Read());
                    ds.Tables.Add(dataTable);
                }

            } while (dataReader.Reader.NextResult());
        }
        return ds;
    }
    public virtual async Task<TEntity> InsertLogEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        using DataConnection dataContext = CreateDataConnection(true);
        entity.Id = await dataContext.InsertWithInt32IdentityAsync(entity);
        return entity;
    }
    public virtual async Task BulkInsertLogEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        using DataConnection dataContext = CreateDataConnection(true);
        await dataContext.BulkCopyAsync(new BulkCopyOptions(), entities.RetrieveIdentity(dataContext));
    }
    public virtual DataSet ExecuteLogStoredProcedureForDataSet(string procedureName, params DataParameter[] parameters)
    {
        using DataConnection dataContext = CreateDataConnection(true);
        dataContext.CommandTimeout = 200;
        System.Data.DataSet ds = new();

        using (DataReader dataReader = dataContext.ExecuteReader(procedureName, System.Data.CommandType.StoredProcedure, System.Data.CommandBehavior.Default, parameters))
        {
            do
            {
                if (dataReader.Reader.Read())
                {
                    var dataTable = new DataTable();
                    string[] fieldNames = Enumerable.Range(0, dataReader.Reader.FieldCount).Select(i => dataReader.Reader.GetName(i)).ToArray();
                    foreach (string field in fieldNames)
                    {
                        dataTable.Columns.Add(field);
                    }

                    do
                    {
                        DataRow dr = dataTable.NewRow();
                        for (int index = 0; index < fieldNames.Length; index++)
                        {
                            dr[fieldNames[index]] = dataReader.Reader.GetValue(index);
                        }
                        dataTable.Rows.Add(dr);
                    } while (dataReader.Reader.Read());
                    ds.Tables.Add(dataTable);
                }

            } while (dataReader.Reader.NextResult());
        }
        return ds;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Linq2Db data provider
    /// </summary>
    protected abstract IDataProvider LinqToDbDataProvider { get; }

    /// <summary>
    /// Database connection string
    /// </summary>
    protected static string GetCurrentConnectionString()
    {
        return DataSettingsManager.LoadSettings().ConnectionString;
    }

    /// <summary>
    /// Log Database connection string
    /// </summary>
    protected static string GetLogConnectionString()
    {
        return DataSettingsManager.LoadSettings().LogConnectionString;
    }
    /// <summary>
    /// External Database connection string
    /// </summary>
    protected static string GetExternalConnectionString()
    {
        return DataSettingsManager.LoadSettings().ExternalDbConnectionString;
    }

    /// <summary>
    /// Name of database provider
    /// </summary>
    public string ConfigurationName => LinqToDbDataProvider.Name;

    #endregion
}