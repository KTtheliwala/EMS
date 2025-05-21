using Newtonsoft.Json;
using TheTecniQ.Core.Domain.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTecniQ.Services.Extensions
{
    public static class LogExtensions
    {
        //public static List<Logs> GetAuditLog<TEntity>(this IList<TEntity> entities, EnumLogAction logAction, int userId, string username)
        //{
        //    return entities.Select(curEntity => curEntity.GetLogObject(logAction, userId, username)).ToList();

        //}
        //public static Logs GetAuditLog<TEntity>(this TEntity entity, EnumLogAction logAction, int userId, string username)
        //{
        //    return entity.GetLogObject(logAction, userId, username);
        //}
        //private static Logs GetLogObject<TEntity>(this TEntity entity, EnumLogAction logAction, int userId, string username)
        //{
        //    SimpleDictionaryStorage externalStorage = new();
        //    AuditLogContractResolver contractResolver = new(externalStorage);

        //    string result = JsonConvert.SerializeObject(entity, new JsonSerializerSettings { ContractResolver = contractResolver });
        //    return new Logs()
        //    {
        //        ActionId = (int)logAction,
        //        ActionById = userId,
        //        ActionByName = username,
        //        EntityId = GetEntityId(entity),
        //        EntityName = typeof(TEntity).Name,
        //        Description = result,
        //        LogTypeId = (int)EnumLogType.Audit,
        //        LogSourceId = (int)EnumLogSource.API,
        //        CreatedOn = DateTime.UtcNow
        //    };
        //}
        public static int GetEntityId<TEntity>(TEntity entity)
        {
            try
            {
                return Convert.ToInt32(typeof(TEntity).GetProperty("Id").GetValue(entity));
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
