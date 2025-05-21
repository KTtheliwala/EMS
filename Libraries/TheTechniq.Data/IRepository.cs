using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;

using TheTecniQ.Core;
using System.Data;
using LinqToDB.Data;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Core;

namespace TheTecniQ.Data;

/// <summary>
/// Represents an entity repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public partial interface IRepository<TEntity> where TEntity : BaseEntity
{
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
    Task<TEntity> GetByIdAsync(int? id, bool includeDeleted = true);

    /// <summary>
    /// Get the entity entry
    /// </summary>
    /// <param name="id">Entity entry identifier</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>
    /// The entity entry
    /// </returns>
    TEntity GetById(int? id, bool includeDeleted = true);

    /// <summary>
    /// Get entity entries by identifiers
    /// </summary>
    /// <param name="ids">Entity entry identifiers</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entries
    /// </returns>
    Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, bool includeDeleted = false);

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="func">Function to select entries</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entries
    /// </returns>
    Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool includeDeleted = false);

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="func">Function to select entries</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <returns>Entity entries</returns>
    IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool includeDeleted = false);

    /// <summary>
    /// Get all entity entries
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
    Task<IPagedList<TEntity>> GetAllPagedAsync(GridRequestModel objGrid, IQueryable<TEntity> query = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);



    /// <summary>
    /// Insert the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<TEntity> InsertAsync(TEntity entity, int UserId, string Username);

    /// <summary>
    /// Insert entity entries
    /// </summary>
    /// <param name="entities">Entity entries</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertAsync(IList<TEntity> entities, int UserId, string Username);

    /// <summary>
    /// Update the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateAsync(TEntity entity, int UserId, string Username);

    /// <summary>
    /// Update entity entries
    /// </summary>
    /// <param name="entities">Entity entries</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateAsync(IList<TEntity> entities, int UserId, string Username);

    /// <summary>
    /// Delete the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Delete entity entries
    /// </summary>
    /// <param name="entities">Entity entries</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteAsync(IList<TEntity> entities, int UserId, string Username);

    /// <summary>
    /// Loads the original copy of the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the copy of the passed entity entry
    /// </returns>
    Task<TEntity> LoadOriginalCopyAsync(TEntity entity);

    /// <summary>
    /// Truncates database table
    /// </summary>
    /// <param name="resetIdentity">Performs reset identity column</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task TruncateAsync(bool resetIdentity = false);

    /// <summary>
    /// Executes command asynchronously and returns number of affected records
    /// </summary>
    /// <param name="sql">Command text</param>
    /// <param name="dataParameters">Command parameters</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of records, affected by command execution.
    /// </returns>
    Task<int> ExecuteNonQueryAsync(string sql, params DataParameter[] dataParameters);

    /// <summary>
    /// Executes command asynchronously and returns single entity
    /// </summary>
    /// <param name="sql">Command text</param>
    /// <param name="parameters">Command parameters</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the value of specified type
    /// </returns>
    Task<T> ExecuteNonQueryAsync<T>(string sql, params DataParameter[] dataParameters);

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
    Task<IList<T>> QueryProcAsync<T>(string procedureName, params DataParameter[] dataParameters);

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
    Task<T> QueryProcEntityAsync<T>(string procedureName, params DataParameter[] dataParameters);

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
    Task<IList<T>> QueryAsync<T>(string sql, params DataParameter[] dataParameters);

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
    Task<T> QueryEntityAsync<T>(string sql, params DataParameter[] dataParameters);
    //
    //Task<T> ExecuteSqlQueryForDataSetAsync<T>(string sql, params DataParameter[] dataParameters);

    Task<QueryMultiResult<T1, T2>> QueryProcMultipleAsync<T1, T2>(string procedureName, params DataParameter[] dataParameters);

    List<T> ConvertDataTable<T>(DataTable dt, string[] skippedProps = null);

    #endregion
    void InsertAuditLog(TEntity Entity, EnumLogAction Action, int UserId, string Username);
    void InsertAuditLog(IList<TEntity> Entities, EnumLogAction Action, int UserId, string Username);
    Task InsertErrorLogAsync(Exception ex, string ErrorMessage, int UserId, string Username);
    Task InsertInformationLogAsync(string Message, int UserId, string Username);
    Task InsertWarningLogAsync(string Message, int UserId, string Username);
    #region Properties

    /// <summary>
    /// Gets a table
    /// </summary>
    IQueryable<TEntity> Table { get; }

    #endregion

    Task<TEntity> GetLogByIdAsync(int? id);
}