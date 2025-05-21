using System;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using LinqToDB.Data;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Data.Extensions;
using Newtonsoft.Json;

using Serilog.Context;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Domain.Logging;
using FluentMigrator.Runner.Generators.Base;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Data;

/// <summary>
/// Represents the entity repository implementation
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public partial class EntityRepository<TEntity>(IDatabaseProvider dataProvider) : IRepository<TEntity> where TEntity : BaseEntity
{
    #region Fields

    protected readonly IDatabaseProvider _dataProvider = dataProvider;

    #endregion

    #region Utilities

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="getAllAsync">Function to select entries</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entries
    /// </returns>
    protected virtual async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync)
    {
        return await getAllAsync();
    }

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="getAll">Function to select entries</param>
    /// <returns>Entity entries</returns>
    protected virtual IList<TEntity> GetEntities(Func<IList<TEntity>> getAll)
    {
        return getAll();
    }

    /// <summary>
    /// Adds "deleted" filter to query which contains <see cref="ISoftDeletedEntity"/> entries, if its need
    /// </summary>
    /// <param name="query">Entity entries</param>
    /// <param name="includeDeleted">Whether to include deleted items</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>Entity entries</returns>
    protected virtual IQueryable<TEntity> AddDeletedFilter(IQueryable<TEntity> query, in bool includeDeleted)
    {
        if (includeDeleted)
            return query;

        if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) == null)
            return query;

        return query.OfType<ISoftDeletedEntity>().Where(entry => !entry.IsDeleted).OfType<TEntity>();
    }


    /// <summary>
    /// Prepare column expression for query.
    /// </summary>
    /// <typeparam name="TV">Field type.</typeparam>
    /// <param name="columnName">Source query column name.</param>
    /// <returns>Expression</returns>
    protected static Expression<Func<TEntity, TV>> CreateColumnExpression<TV>(string columnName)
    {
        // Get the entity type
        Type entityType = typeof(TEntity);

        // Find the property info based on the column name
        PropertyInfo propertyInfo = entityType.GetProperty(columnName);

        // Create the parameter expression for the entity
        var parameter = Expression.Parameter(entityType, "x");

        // Create the property expression
        var property = Expression.Property(parameter, propertyInfo);

        // Create the lambda expression
        return Expression.Lambda<Func<TEntity, TV>>(property, parameter);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get the entity entry
    /// </summary>
    /// <param name="id">Entity entry identifier</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entry
    /// </returns>
    public virtual async Task<TEntity> GetByIdAsync(int? id, bool includeDeleted = true)
    {
        if (!id.HasValue || id == 0)
            return null;

        async Task<TEntity> getEntityAsync()
        {
            return await AddDeletedFilter(Table, includeDeleted).FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt32(id));
        }

        return await getEntityAsync();
    }

    /// <summary>
    /// Get the entity entry
    /// </summary>
    /// <param name="id">Entity entry identifier</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>
    /// The entity entry
    /// </returns>
    public virtual TEntity GetById(int? id, bool includeDeleted = true)
    {
        if (!id.HasValue || id == 0)
            return null;

        TEntity getEntity()
        {
            return AddDeletedFilter(Table, includeDeleted).FirstOrDefault(entity => entity.Id == Convert.ToInt32(id));
        }

        return getEntity();
    }

    /// <summary>
    /// Get entity entries by identifiers
    /// </summary>
    /// <param name="ids">Entity entry identifiers</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entries
    /// </returns>
    public virtual async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, bool includeDeleted = false)
    {
        if (ids?.Any() != true)
            return [];

        static IList<TEntity> sortByIdList(IList<int> listOfId, IDictionary<int, TEntity> entitiesById)
        {
            var sortedEntities = new List<TEntity>(listOfId.Count);

            foreach (var id in listOfId)
                if (entitiesById.TryGetValue(id, out var entry))
                    sortedEntities.Add(entry);

            return sortedEntities;
        }

        async Task<IList<TEntity>> getByIdsAsync(IList<int> listOfId, bool sort = true)
        {
            var query = AddDeletedFilter(Table, includeDeleted).Where(entry => listOfId.Contains(entry.Id));

            return sort
                ? sortByIdList(listOfId, await query.ToDictionaryAsync(entry => entry.Id))
                : await query.ToListAsync();
        }

        return await getByIdsAsync(ids);
    }

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="func">Function to select entries</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entries
    /// </returns>
    public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool includeDeleted = false)
    {
        async Task<IList<TEntity>> getAllAsync()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func != null ? func(query) : query;

            return await query.ToListAsync();
        }

        return await GetEntitiesAsync(getAllAsync);
    }

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="func">Function to select entries</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>Entity entries</returns>
    public virtual IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool includeDeleted = false)
    {
        IList<TEntity> getAll()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func != null ? func(query) : query;

            return [.. query];
        }

        return GetEntities(getAll);
    }

    /// <summary>
    /// Get paged list of all entity entries
    /// </summary>
    /// <param name="func">Function to select entries</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">Whether to get only the total number of entries without actually loading data</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <param name="ignorePaging">Returl all records without paging.</param>
    /// <param name="isLogDB">Log database table</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the paged list of entity entries
    /// </returns>
    public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(GridRequestModel objGrid, IQueryable<TEntity> query = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
    {
        query ??= Table;
        query = func != null ? func(query) : query;
        return await query.BuildPredicate(objGrid);
    }


    /// <summary>
    /// Insert the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<TEntity> InsertAsync(TEntity entity, int UserId, string Username)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var result = await _dataProvider.InsertEntityAsync(entity);
        InsertAuditLog(result, EnumLogAction.Add, UserId, Username);
        return result;
    }

    /// <summary>
    /// Insert entity entries
    /// </summary>
    /// <param name="entities">Entity entries</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertAsync(IList<TEntity> entities, int UserId, string Username)
    {
        ArgumentNullException.ThrowIfNull(entities);
        await _dataProvider.BulkInsertEntitiesAsync(entities);
        foreach (var entity in entities)
        {
            InsertAuditLog(entity, EnumLogAction.Add, UserId, Username);
        }

    }

    /// <summary>
    /// Loads the original copy of the entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the copy of the passed entity
    /// </returns>
    public virtual async Task<TEntity> LoadOriginalCopyAsync(TEntity entity)
    {
        return await _dataProvider.GetTable<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(entity.Id));
    }

    public virtual LinqToDB.Linq.IUpdatable<TEntity> GetSetter(IQueryable<TEntity> query, LinqToDB.Linq.IUpdatable<TEntity> setter, string columnName, object newColumnValue)
    {
        if (newColumnValue is int)
        {
            return SetValue<int>(query, setter, columnName, Convert.ToInt32(newColumnValue));
        }
        else if (newColumnValue is float)
        {
            return SetValue<float>(query, setter, columnName, Convert.ToSingle(newColumnValue));
        }
        else if (newColumnValue is double)
        {
            return SetValue<double>(query, setter, columnName, Convert.ToDouble(newColumnValue));
        }
        else if (newColumnValue is decimal)
        {
            return SetValue<decimal>(query, setter, columnName, Convert.ToDecimal(newColumnValue));
        }
        else if (newColumnValue is bool)
        {
            return SetValue<bool>(query, setter, columnName, Convert.ToBoolean(newColumnValue));
        }
        else
        {
            return SetValue<string>(query, setter, columnName, Convert.ToString(newColumnValue));
        }
    }
    public virtual LinqToDB.Linq.IUpdatable<TEntity> SetValue<T>(IQueryable<TEntity> query, LinqToDB.Linq.IUpdatable<TEntity> setter, string columnName, T newColumnValue)
    {
        var exp = CreateColumnExpression<T>(columnName);
        return setter == null ? LinqToDB.LinqExtensions.Set(query, exp, newColumnValue) : LinqToDB.LinqExtensions.Set(setter, exp, newColumnValue);
    }

    /// <summary>
    /// Update the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateAsync(TEntity entity, int UserId, string Username)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _dataProvider.UpdateEntityAsync(entity);
        InsertAuditLog(entity, EnumLogAction.Edit, UserId, Username);
    }
    /// <summary>
    /// Update entity entries
    /// </summary>
    /// <param name="entities">Entity entries</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateAsync(IList<TEntity> entities, int UserId, string Username)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (!entities.Any())
            return;

        await _dataProvider.UpdateEntitiesAsync(entities);
        foreach (TEntity entity in entities)
        {
            InsertAuditLog(entity, EnumLogAction.Edit, UserId, Username);
        }
    }

    /// <summary>
    /// Delete the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        switch (entity)
        {
            case ISoftDeletedEntity softDeletedEntity:
                softDeletedEntity.IsDeleted = true;
                await _dataProvider.UpdateEntityAsync(entity);
                break;

            default:
                await _dataProvider.DeleteEntityAsync(entity);
                break;
        }

    }

    /// <summary>
    /// Delete entity entries
    /// </summary>
    /// <param name="entities">Entity entries</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteAsync(IList<TEntity> entities, int UserId, string Username)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (!entities.Any())
            return;

        if (entities.OfType<ISoftDeletedEntity>().Any())
        {
            entities.OfType<ISoftDeletedEntity>().ToList().ConvertAll(x => x.IsDeleted = true);
            await _dataProvider.UpdateEntitiesAsync(entities);
        }
        else
        {
            await _dataProvider.BulkDeleteEntitiesAsync(entities);
        }
        foreach (var entity in entities)
        {
            InsertAuditLog(entity, EnumLogAction.Delete, UserId, Username);
        }
    }

    /// <summary>
    /// Delete entity entries by the passed predicate
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <returns>
    /// The number of deleted records
    /// </returns>
    public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        var countDeletedRecords = _dataProvider.BulkDeleteEntities(predicate);
        return countDeletedRecords;
    }

    /// <summary>
    /// Truncates database table
    /// </summary>
    /// <param name="resetIdentity">Performs reset identity column</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task TruncateAsync(bool resetIdentity = false)
    {
        await _dataProvider.TruncateAsync<TEntity>(resetIdentity);
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
        return await _dataProvider.ExecuteNonQueryAsync(sql, dataParameters);
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
        return await _dataProvider.ExecuteNonQueryAsync<T>(sql, dataParameters);
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
        return await _dataProvider.QueryProcAsync<T>(procedureName, dataParameters);
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
        return await _dataProvider.QueryProcEntityAsync<T>(procedureName, dataParameters);
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
    public virtual async Task<IList<T>> QueryAsync<T>(string sql, params DataParameter[] dataParameters)
    {
        return await _dataProvider.QueryAsync<T>(sql, dataParameters);
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
    public virtual async Task<T> QueryEntityAsync<T>(string sql, params DataParameter[] dataParameters)
    {
        return await _dataProvider.QueryEntityAsync<T>(sql, dataParameters);
    }
    
    public virtual async Task<QueryMultiResult<T1, T2>> QueryProcMultipleAsync<T1, T2>(string procedureName, params DataParameter[] dataParameters)
    {
        return await _dataProvider.QueryProcMultipleAsync<T1, T2>(procedureName, dataParameters);
    }
    public virtual List<T> ConvertDataTable<T>(DataTable dt, string[] skippedProps = null)
    {
        List<T> data = [];
        foreach (DataRow row in dt.Rows)
        {
            T item = CoreCommonExtensions.GetItemNew<T>(row, skippedProps);
            data.Add(item);
        }
        return data;
    }

    #endregion
    public void InsertAuditLog(TEntity Entity, EnumLogAction Action, int UserId, string Username)
    {
        if (UserId > 0)
        {
            LogContext.PushProperty("Entity", typeof(TEntity).Name);
            LogContext.PushProperty("EntityId", Entity.Id.ToString());
            LogContext.PushProperty("ActionId", (int)Action);
            LogContext.PushProperty("LogTypeId", (int)EnumLogType.Audit);
            LogContext.PushProperty("UserId", UserId);
            LogContext.PushProperty("Username", Username);
            Serilog.Log.Write(Serilog.Events.LogEventLevel.Information + 1000, JsonConvert.SerializeObject(Entity));
        }
    }
    public void InsertAuditLog(IList<TEntity> Entities, EnumLogAction Action, int UserId, string Username)
    {
        if (UserId > 0)
        {
            foreach (TEntity entity in Entities)
            {

                LogContext.PushProperty("Entity", typeof(TEntity).Name);
                LogContext.PushProperty("EntityId", entity.Id.ToString());
                LogContext.PushProperty("ActionId", (int)Action);
                LogContext.PushProperty("LogTypeId", (int)EnumLogType.Audit);
                LogContext.PushProperty("UserId", UserId);
                LogContext.PushProperty("Username", Username);
                Serilog.Log.Write(Serilog.Events.LogEventLevel.Information + 1000, JsonConvert.SerializeObject(entity));
            }
        }
    }
    public async Task InsertErrorLogAsync(Exception ex, string ErrorMessage, int UserId, string Username)
    {
        LogContext.PushProperty("LogTypeId", (int)EnumLogType.Error);
        LogContext.PushProperty("UserId", UserId);
        LogContext.PushProperty("Username", Username);
        Serilog.Log.Error(ex, ErrorMessage);
        await Serilog.Log.CloseAndFlushAsync();
    }
    public async Task InsertInformationLogAsync(string Message, int UserId, string Username)
    {
        LogContext.PushProperty("LogTypeId", (int)EnumLogType.Error);
        LogContext.PushProperty("UserId", UserId);
        LogContext.PushProperty("Username", Username);
        Serilog.Log.Information(Message);
        await Serilog.Log.CloseAndFlushAsync();
    }
    public async Task InsertWarningLogAsync(string Message, int UserId, string Username)
    {
        LogContext.PushProperty("LogTypeId", (int)EnumLogType.Error);
        LogContext.PushProperty("UserId", UserId);
        LogContext.PushProperty("Username", Username);
        Serilog.Log.Warning(Message);
        await Serilog.Log.CloseAndFlushAsync();
    }

    #region Properties

    /// <summary>
    /// Gets a table
    /// </summary>
    public virtual IQueryable<TEntity> Table => _dataProvider.GetTable<TEntity>();

    /// <summary>
    /// Gets a table
    /// </summary>
    public virtual IQueryable<TEntity> LogTable => _dataProvider.GetTable<TEntity>(true);

    #endregion

    public virtual async Task<TEntity> GetLogByIdAsync(int? id)
    {
        if (!id.HasValue || id == 0)
        {
            return null;
        }

        async Task<TEntity> getEntityAsync()
        {
            return await LogTable.FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt32(id));
        }

        return await getEntityAsync();
    }
}