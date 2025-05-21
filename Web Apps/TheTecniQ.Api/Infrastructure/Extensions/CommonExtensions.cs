using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using System.Data;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheTecniQ.Core.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;
using TheTecniQ.Core.Domain.User;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TheTecniQ.Core.Configuration;
using Microsoft.AspNetCore.Http;
using TheTecniQ.Core.Domain.Permissions;
using Newtonsoft.Json.Linq;
using System.IO;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core;
using ExcelDataReader;
using System.Data;
using DataTable = System.Data.DataTable;
using System.Xml.Linq;
using EasyXLS;
using Aspose.Cells;

namespace TheTecniQ.API.Infrastructure.Extensions
{
    public static class CommonExtensions
    {
        public static HttpContext HttpContextAccessor => new HttpContextAccessor().HttpContext;
        private static readonly DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static int ToInt(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static string ToDynamicString(this decimal? value)
        {
            if (!value.HasValue)
                return ""; // Default value for null

            return value.Value % 1 == 0
        ? value.Value.ToString("#,0")  // Integer format with commas
        : value.Value.ToString("#,0.##"); // 
        }
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static double ToDouble(this object a)
        {
            try
            {
                return Convert.ToDouble(a);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void CopyProperties<EntityBase>(this EntityBase source, EntityBase destination, string[] Skipped = null)
        {
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();
            foreach (PropertyInfo destinationPi in destinationProperties)
            {
                if (Skipped == null || !Skipped.Contains(destinationPi.Name))
                {
                    PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
                    destinationPi.SetValue(destination, sourcePi.GetValue(source, null), null);
                }
            }
        }


        //Security

        public static string Encrypt(this string plainText)
        {
            try
            {
                return EncryptionUtility.Encrypt(plainText, AppConfig.AESKeys.Key, AppConfig.AESKeys.IV);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string Descrypt(this string cipherText)
        {
            try
            {
                return EncryptionUtility.Decrypt(cipherText, AppConfig.AESKeys.Key, AppConfig.AESKeys.IV);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get SHA256 hash
        /// </summary>
        /// <param name="plainText">Text to hash</param>
        /// <returns>SHA256 hash</returns>
        public static string Hash(this string plainText)
        {
            try
            {
                return EncryptionUtility.CreateHash(plainText);
            }
            catch (Exception)
            {
                return null;
            }
        }


        //List

        public static List<int> ToIntList(this object a, string separator)
        {
            try
            {
                return string.IsNullOrWhiteSpace(Convert.ToString(a)) ? [] : a.ToString().Split(separator.ToCharArray()).Select(int.Parse).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                //--------- change from here
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
                //--------- change up to here
            }
            return dynamicDt;
        }


        //Enum

        /// <summary>
        /// From enum type convert to SelectListItems
        /// </summary>
        /// <param name="enumType">Type of enum</param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItems(this Type enumType)
        {
            List<SelectListItem> items = [];
            foreach (Enum cur in Enum.GetValues(enumType))
            {
                items.Add(new SelectListItem()
                {
                    Text = cur.ToString().Replace('_', ' '),
                    Value = GetEnumValue(cur)
                });
            }
            return items;
        }

        public static List<SelectListItem> ToSelectListItemsDescription(this Type enumType, bool IsDescription = false)
        {
            List<SelectListItem> items = [];
            foreach (Enum cur in Enum.GetValues(enumType))
            {
                items.Add(new SelectListItem()
                {
                    Text = IsDescription ? cur.ToDescription() : cur.ToString(),
                    Value = GetEnumValue(cur)
                });
            }
            return items;
        }

        public static List<SelectListItem> ToSelectListItemsDisplay(this Type enumType, string Country = "")
        {
            List<SelectListItem> items = [];
            if (!string.IsNullOrWhiteSpace(Country))
            {
                if (Country.ToLower() == "my")
                {
                    foreach (Enum cur in Enum.GetValues(enumType))
                    {
                        items.Add(new SelectListItem()
                        {
                            Text = cur.ToDisplayName().ToString().Replace('_', ' ').Replace("$", "RM"),
                            Value = GetEnumValue(cur)
                        });
                    }
                    return items;
                }
            }

            foreach (Enum cur in Enum.GetValues(enumType))
            {
                items.Add(new SelectListItem()
                {
                    Text = cur.ToDisplayName().ToString().Replace('_', ' '),
                    Value = GetEnumValue(cur)
                });
            }
            return items;
        }

        public static string GetEnumValue(this Enum EnumType)
        {
            return Convert.ToString((int)(object)EnumType);
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
        public static string GetTokenValue(IPrincipal user, string climType)
        {
            return EncryptionUtility.Decrypt((user.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == climType).Value, AppConfig.AESKeys.Key, AppConfig.AESKeys.IV);
        }
        public static string GenerateToken(EMS_User user, EMS_User profile, string Secret, int Minutes, string permissions, string Issuer, string Audience)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(Secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                               new("Id",EncryptionUtility.Encrypt(((user != null)?user.Id.ToString():profile.Id.ToString()),AppConfig.AESKeys.Key,AppConfig.AESKeys.IV)),
                               new("Permissions",EncryptionUtility.Encrypt(((user != null)?permissions:""),AppConfig.AESKeys.Key,AppConfig.AESKeys.IV)),
                               new("UserToken",EncryptionUtility.Encrypt(((user != null)?user.UserToken:user.UserToken),AppConfig.AESKeys.Key,AppConfig.AESKeys.IV)),
                               new("RoleId", EncryptionUtility.Encrypt((user != null)?user.RoleId.ToString():"",AppConfig.AESKeys.Key,AppConfig.AESKeys.IV)),
                               new("UserName", EncryptionUtility.Encrypt((user != null)?user.UserName:profile.UserName ,AppConfig.AESKeys.Key,AppConfig.AESKeys.IV)),
                               new("UserType",EncryptionUtility.Encrypt(((user != null)?"Admin":"Branch"),AppConfig.AESKeys.Key,AppConfig.AESKeys.IV)),
                               new("FullName", EncryptionUtility.Encrypt(((user != null)?(user.FirstName + (!string.IsNullOrEmpty(user.LastName) ? " " + user.LastName : "")):profile.UserName) ,AppConfig.AESKeys.Key,AppConfig.AESKeys.IV))
                }),
                Issuer = Issuer,
                Audience = Audience,
                Expires = DateTime.UtcNow.AddMinutes(Minutes <= 0 ? 30 : Minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
        //public static string GenerateProfileToken(ProfileMaster user, string Secret, int Minutes, string permissions)
        //{
        //    JwtSecurityTokenHandler tokenHandler = new();
        //    byte[] key = Encoding.ASCII.GetBytes(Secret);
        //    SecurityTokenDescriptor tokenDescriptor = new()
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //                       new("Id",EncryptionUtility.Encrypt(user.Id.ToString(),AppConfig.AESKeys.Key,AppConfig.AESKeys.IV)),                               
        //                       new("UserToken",EncryptionUtility.Encrypt(user.UserToken,AppConfig.AESKeys.Key,AppConfig.AESKeys.IV)),                               
        //                       new("Name",EncryptionUtility.Encrypt(user.Name,AppConfig.AESKeys.Key,AppConfig.AESKeys.IV))                               
        //        }),
        //        Expires = DateTime.UtcNow.AddMinutes(Minutes <= 0 ? 30 : Minutes),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        //}
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string Key, string Issuer, string Audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {              
                var key = Encoding.ASCII.GetBytes(Key);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false // Allow expired tokens
                };
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }
                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }            
            
        }
        public static int GetId(HttpRequest request)
        {
            int Id = 0;

            HttpRequestRewindExtensions.EnableBuffering(request);
            try
            {
                if ((request?.ContentType?.Contains("form-data") ?? false))
                {
                    Id = request.Form["Id"].ToString().ToInt();
                }
                else
                {
                    using StreamReader reader = new(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                    string strRequestBody = reader.ReadToEnd();
                    Id = JObject.Parse(strRequestBody)["Id"].ToInt();
                }
            }
            catch (Exception) { }
            finally
            {
                request.Body.Position = 0;
            }
            return Id;
        }
        public static bool CheckPermission(string pageCode, PagePermission permit)
        {
            ClaimsPrincipal currentUser = HttpContextAccessor.User;
            if (currentUser.HasClaim(c => c.Type == "RoleId"))
            {
                if (currentUser.HasClaim(c => c.Type == "Permissions"))
                {
                    if (permit == PagePermission.AddOrEdit)
                    {
                        int Id = GetId(HttpContextAccessor.Request);
                        permit = Id > 0 ? PagePermission.Edit : PagePermission.Add;
                    }

                    string permissions = EncryptionUtility.Decrypt(currentUser.Claims.FirstOrDefault(c => c.Type == "Permissions").Value, AppConfig.AESKeys.Key, AppConfig.AESKeys.IV);
                    string[] AllPermissions = permissions.Split(',');
                    foreach (string permission in AllPermissions)
                    {
                        if (permission.StartsWith(pageCode + "|"))
                        {
                            string[] perms = permission.Split('|');
                            if (permit == PagePermission.Add && perms.Length > 1)
                            {
                                return perms[1] == "1";
                            }
                            else if (permit == PagePermission.Edit && perms.Length > 2)
                            {
                                return perms[2] == "1";
                            }
                            else if (permit == PagePermission.Delete && perms.Length > 3)
                            {
                                return perms[3] == "1";
                            }
                            else if (permit == PagePermission.View && perms.Length > 4)
                            {
                                return perms[4] == "1";
                            }
                            else if (permit == PagePermission.Update && perms.Length > 5)
                            {
                                return perms[5] == "1";
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static DataTable ExtractExcel(string fileName, string filePath)
        {            
            DataTable objDataTableItems = null;
            string excelPath = filePath + "\\" + fileName;

            try
            {
                using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Use the first result set (worksheet) from the Excel file
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // Use the first row as column names
                            }
                        });

                        // Return the first DataTable from the result set
                        return result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       public static void DeleteAllFiles(string directoryPath)
        {
            try
            {
                // Check if the directory exists
                if (Directory.Exists(directoryPath))
                {
                    // Get all files in the directory
                    string[] files = Directory.GetFiles(directoryPath);

                    // Iterate over the files and delete each one
                    foreach (string file in files)
                    {
                        File.Delete(file);
                        Console.WriteLine($"Deleted file: {file}");
                    }

                    Console.WriteLine("All files have been deleted.");
                }
                else
                {
                    Console.WriteLine($"The directory '{directoryPath}' does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public static DataTable ExtractExcel1(string fileName, string filePath, bool convertToXlsx= false)
        {
            string excelPath = filePath + "\\" + fileName;
            if (convertToXlsx)
            {
                string newfilePath = filePath + "\\" + fileName + "x";
                var workbook = new Workbook(excelPath);
                workbook.Save(newfilePath);
                excelPath = newfilePath;
                return ExtractExcel(fileName + "x", filePath);
            }

            DataTable dt = new DataTable();
          
            try
            {
                XDocument xdoc = XDocument.Load(excelPath);
                DataTable dataTable = ConvertXmlToDataTable(xdoc);

                // Display DataTable contents
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        Console.Write($"{item} ");
                    }
                    Console.WriteLine();
                }
                return dataTable;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        static DataTable ConvertXmlToDataTable(XDocument xdoc)
        {
            XNamespace ns = "urn:schemas-microsoft-com:office:spreadsheet";
            DataTable dataTable = new DataTable();

            var rows = xdoc.Descendants(ns + "Row");

            bool isFirstRow = true;

            foreach (var row in rows)
            {
                DataRow dataRow = dataTable.NewRow();
                var cells = row.Descendants(ns + "Cell");

                int cellIndex = 0;

                foreach (var cell in cells)
                {
                    var data = cell.Descendants(ns + "Data").FirstOrDefault();
                    if (data != null)
                    {
                        if (isFirstRow)
                        {
                            // Add columns on the first row
                            dataTable.Columns.Add(data.Value);
                        }
                        else
                        {
                            dataRow[cellIndex] = data.Value;
                        }
                    }
                    cellIndex++;
                }

                if (isFirstRow)
                {
                    isFirstRow = false;
                }
                else
                {
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        public static bool HasChanges<T>(T existing, T updated, string[] propertiesToCheck)
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                // Skip ignored properties
                if (propertiesToCheck?.Contains(property.Name) != true)
                    continue;

                var existingValue = property.GetValue(existing);
                var updatedValue = property.GetValue(updated);

                if (IsDateTimeProperty(property))
                {
                    if (HasDateChanges(existingValue, updatedValue)) return true;
                }
                else
                {
                    if (HasValueChanges(existingValue, updatedValue)) return true;
                }
            }

            return false;
        }

        static bool IsDateTimeProperty(PropertyInfo property)
        {
            // Check if the property is DateTime or DateTime? type
            return property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?);
        }

        static bool HasDateChanges(object existingValue, object updatedValue)
        {
            // Handle DateTime and nullable DateTime
            var existingDate = existingValue as DateTime?;
            var updatedDate = updatedValue as DateTime?;

            // Compare only the date portion, ignoring time
            return existingDate?.Date != updatedDate?.Date;
        }

        static bool HasValueChanges(object existingValue, object updatedValue)
        {
            // Handle null cases safely and compare values
            if (existingValue == null && updatedValue != null) return true;
            if (existingValue != null && updatedValue == null) return true;
            return !object.Equals(existingValue, updatedValue);
        }
    }
}