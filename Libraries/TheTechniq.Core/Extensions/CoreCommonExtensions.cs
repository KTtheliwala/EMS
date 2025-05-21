using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheTecniQ.Core
{
    public static class CoreCommonExtensions
    {
        public static DateTime ToLocalDateTime(this DateTime OnDate, string Timezone)
        {
            TimeSpan ts = string.IsNullOrEmpty(Timezone) ? new TimeSpan() : TimeSpan.Parse(Timezone.Replace("+", ""));
            return OnDate.Add(ts);
        }
        public static DateTime ToUtcDateTime(this DateTime OnDate, string Timezone)
        {
            TimeSpan ts = string.IsNullOrEmpty(Timezone) ? new TimeSpan() : TimeSpan.Parse(Timezone.Replace("+", ""));
            return OnDate.Subtract(ts);
        }
        public static T GetItemNew<T>(DataRow dr, string[] skippedProps)
        {
            Type type = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                PropertyInfo property = type.GetProperty(column.ColumnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (property != null && property.CanWrite && (skippedProps == null || !skippedProps.Contains(property.Name)))
                {
                    object value = dr[column.ColumnName];
                    if (value != DBNull.Value)
                    {
                        SetPropertyValue(property, obj, value);
                    }
                }
            }
            return obj;
        }

        private static void SetPropertyValue<T>(PropertyInfo property, T obj, object value)
        {
            Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            if (propertyType.IsEnum)
            {
                property.SetValue(obj, Enum.ToObject(propertyType, value));
            }
            else if (propertyType == typeof(int))
            {
                property.SetValue(obj, Convert.ToInt32(value));
            }
            else if (propertyType == typeof(decimal))
            {
                property.SetValue(obj, Convert.ToDecimal(value));
            }
            else if (propertyType == typeof(double))
            {
                property.SetValue(obj, Convert.ToDouble(value));
            }
            else if (propertyType == typeof(bool))
            {
                property.SetValue(obj, Convert.ToBoolean(value));
            }
            else if (propertyType == typeof(DateTime))
            {
                property.SetValue(obj, Convert.ToDateTime(value));
            }
            else
            {
                property.SetValue(obj, value);
            }
        }
    }
}
