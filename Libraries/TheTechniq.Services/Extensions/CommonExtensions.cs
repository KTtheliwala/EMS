using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheTecniQ.Services
{
    public class ServiceCommonExtensions
    {
        public static List<T> ConvertDataTable<T>(DataTable dt, string[] skippedProps = null)
        {
            List<T> data = new();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItemNew<T>(row, skippedProps);
                data.Add(item);
            }
            return data;
        }
        public static T GetItemNew<T>(DataRow dr, string[] skippedProps)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if ((skippedProps?.Length ?? 0) == 0 || !skippedProps.Contains(pro.Name))
                    {
                        object value = dr[column.ColumnName];
                        if (value != DBNull.Value)
                        {
                            if (string.Equals(pro.Name, column.ColumnName, StringComparison.OrdinalIgnoreCase) && pro.CanWrite)
                            {
                                if (pro.PropertyType == typeof(System.Int32) || pro.PropertyType == typeof(System.Int32?))
                                {
                                    if (dr[column.ColumnName] != DBNull.Value)
                                        pro.SetValue(obj, Convert.ToInt32(dr[column.ColumnName]), null);
                                }
                                else if (pro.PropertyType == typeof(System.Decimal) || pro.PropertyType == typeof(System.Decimal?))
                                {
                                    if (dr[column.ColumnName] != DBNull.Value)
                                        pro.SetValue(obj, Convert.ToDecimal(dr[column.ColumnName]), null);
                                }
                                else if (pro.PropertyType == typeof(System.Double) || pro.PropertyType == typeof(System.Double?))
                                {
                                    if (dr[column.ColumnName] != DBNull.Value)
                                        pro.SetValue(obj, Convert.ToDouble(dr[column.ColumnName]), null);
                                }
                                else if (pro.PropertyType == typeof(System.Boolean) || pro.PropertyType == typeof(System.Boolean?))
                                {
                                    if (dr[column.ColumnName] != DBNull.Value)
                                        pro.SetValue(obj, Convert.ToBoolean(dr[column.ColumnName]), null);
                                }
                                else if (pro.PropertyType == typeof(System.DateTime) || pro.PropertyType == typeof(System.DateTime?))
                                {
                                    if (dr[column.ColumnName] != DBNull.Value)
                                        pro.SetValue(obj, Convert.ToDateTime(dr[column.ColumnName]), null);
                                }
                                else
                                    pro.SetValue(obj, dr[column.ColumnName] ?? "", null);
                            }
                            //pro.SetValue(obj, dr[column.ColumnName] ?? "", null);  
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            return obj;
        }

        public static class FileUploadConstants
        {
            public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".gif", ".png" };
            public static readonly string[] AllowedFileExtensions = { ".xlsx", ".pdf", ".csv", ".xls" };
            public const long MaxImageSizeBytes = 5 * 1024 * 1024;
        }

    }
}
