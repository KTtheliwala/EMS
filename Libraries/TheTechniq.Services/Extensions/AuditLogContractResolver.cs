using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Data.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Data;

namespace TheTecniQ.Services.Extensions
{
    public class AuditLogContractResolver(IExternalStorage externalValueStorage) : DefaultContractResolver
    {
        private readonly IExternalStorage _externalValueStorage = externalValueStorage;

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            JsonObjectContract objectContract = base.CreateObjectContract(objectType);
            foreach (JsonProperty jsonProperty in objectContract.Properties.ToArray())
            {
                PropertyInfo propertyInfo = jsonProperty.DeclaringType.GetProperties().SingleOrDefault(p => p.Name == jsonProperty.UnderlyingName);
                if (propertyInfo.GetCustomAttributes(typeof(AuditLogAttribute), inherit: false).SingleOrDefault() != null)
                {
                    if (propertyInfo.GetCustomAttribute<AuditLogAttribute>().Ignore)
                    {
                        jsonProperty.Ignored = true;
                    }
                    else
                    {
                        string PropertyName = propertyInfo.GetCustomAttribute<AuditLogAttribute>().Label;
                        if (string.IsNullOrWhiteSpace(PropertyName))
                        {
                            PropertyName = propertyInfo.Name;
                        }

                        JsonProperty jsonProp = objectContract.Properties.FirstOrDefault(x => x.PropertyName == PropertyName || x.PropertyName == propertyInfo.Name);
                        if (jsonProp != null)
                        {
                            objectContract.Properties.Remove(jsonProp);
                        }
                        objectContract.Properties.Add(new JsonProperty()
                        {
                            PropertyName = PropertyName,
                            PropertyType = typeof(string),
                            ValueProvider = new ExternalStorageValueProvider(_externalValueStorage, propertyInfo),
                            Readable = true,
                            Writable = true
                        });
                    }
                }
                else if (propertyInfo.GetCustomAttributes(typeof(NoColumnMap), inherit: false).SingleOrDefault() != null)
                {
                    jsonProperty.Ignored = true;
                }

            }
            return objectContract;
        }
    }
    public class ExternalStorageValueProvider(IExternalStorage externalValueStorage, PropertyInfo propertyInfo) : IValueProvider
    {
        private readonly IExternalStorage _externalValueStorage = externalValueStorage;
        private readonly PropertyInfo _propertyInfo = propertyInfo;

        public object GetValue(object target)
        {
            object valueRaw = _propertyInfo.GetValue(target);
            if (string.IsNullOrWhiteSpace(Convert.ToString(valueRaw)))
            {
                return "";
            }
            AuditLogAttribute proInfo = _propertyInfo.GetCustomAttribute<AuditLogAttribute>();
            if (proInfo.IsEnum || string.IsNullOrWhiteSpace(proInfo.ReferenceTable))
            {
                if (_propertyInfo.PropertyType == typeof(bool) && !string.IsNullOrWhiteSpace(proInfo.BoolFor))
                {
                    return valueRaw.ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase) ? proInfo.BoolFor.Split('|')[0] : proInfo.BoolFor.Split('|')[1];
                }
                else if (_propertyInfo.PropertyType == typeof(DateTime))
                {
                    string format = proInfo.DateFormat;
                    if (string.IsNullOrWhiteSpace(format))
                    {
                        format = "dd/MM/yyyy HH:mm";
                    }

                    return string.IsNullOrWhiteSpace(Convert.ToString(valueRaw)) ? "" : Convert.ToDateTime(valueRaw).ToString(format);
                }
                return Convert.ToString(valueRaw);
            }
            try
            {
                MsSqlDataProvider obj = new();
                DataParameter[] db =
                [
                    new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@tblName", Size = -1, Value = proInfo.ReferenceTable },
                    new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@colName", Size = -1, Value = proInfo.ReferenceColumn },
                    new DataParameter() { DataType = LinqToDB.DataType.VarChar, Name = "@Value", Size = -1, Value = valueRaw.ToString() },
                ];
                IList<LogReffResult> data = obj.QueryProcAsync<LogReffResult>("Get_Reff_Log", db).Result;
                if (data.Count > 0)
                {
                    return string.Join(',', data.Select(x => x.Name));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                //Utilities.CommonExtensions.WriteToFile("Error on Log:Msg=" + ex.Message + "||Inner Msg=" + ex.InnerException);
                return null;
            }
        }
        public void SetValue(object target, object value)
        {
            Guid id = (Guid)value;
            string valueJson = _externalValueStorage.GetValue(id);
            object valueRaw = JsonConvert.DeserializeObject(valueJson);
            _propertyInfo.SetValue(target, valueRaw);
        }
    }
    public interface IExternalStorage
    {
        Guid SetValue(string objectAsJson);
        string GetValue(Guid id);
    }
    public class SimpleDictionaryStorage : IExternalStorage
    {
        private readonly Dictionary<Guid, string> _store = [];
        public Guid SetValue(string objectAsJsonString)
        {
            Guid id = Guid.NewGuid();
            _store[id] = objectAsJsonString;
            return id;
        }
        public string GetValue(Guid id)
        {
            return _store[id];
        }
    }
}
