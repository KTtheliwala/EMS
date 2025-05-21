using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using ClosedXML.Excel;
using TheTecniQ.Api.Controllers.Login;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.API.Models.Common;
using System.Linq;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel.Drawings;
using System.IO.Pipes;
using System.Drawing;
using System.Data;
using RabbitMQ.Client;
using TheTecniQ.Api.Models.Common;
using System.Text;
using System.Net;

namespace TheTecniQ.Api.Infrastructure
{
    public static class ImportExcelHelper
    {
        public static async Task<List<T>> GetData<T>(ImportExcelDto obj, string fileUploadBasePath, string fileFolderName)
        {
            var data = new List<T>();

            if (IsFileValid(obj.File))
            {
                string fileName = await UploadFileAsync(obj, fileUploadBasePath, fileFolderName);
                using var excelWorkbook = LoadExcelWorkbook(fileUploadBasePath, fileFolderName, fileName);

                var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();
                ProcessDataRows<T>(obj, data, nonEmptyDataRows);
                DeleteUploadedFile(fileUploadBasePath, fileFolderName, fileName);
            }

            return data;
        }
        private static bool IsFileValid(IFormFile file)
        {
            return file != null && file.Length > 0;
        }
        private static void ProcessDataRows<T>(ImportExcelDto obj, List<T> data, IEnumerable<IXLRow> nonEmptyDataRows)
        {
            int startRow = 2;

            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() == 1)
                {
                    MapColumns(obj, dataRow);
                }
                if (dataRow.RowNumber() >= startRow)
                {
                    T objItem = CreateObjectFromRow<T>(obj, dataRow);
                    var property = objItem.GetType().GetProperty("SrNo");
                    if (property != null)
                    {
                        property?.SetValue(objItem, (dataRow.RowNumber() - 1));
                    }
                    data.Add(objItem);
                }
            }
        }

        private static async Task<string> UploadFileAsync(ImportExcelDto obj, string fileUploadBasePath, string fileFolderName)
        {
            return await FileUploadHelper.UploadFileAsync(obj.File, fileUploadBasePath, fileFolderName);
        }

        private static XLWorkbook LoadExcelWorkbook(string fileUploadBasePath, string fileFolderName, string fileName)
        {
            return new XLWorkbook(Path.Combine(fileUploadBasePath, fileFolderName, fileName));
        }

        private static void MapColumns(ImportExcelDto obj, IXLRow dataRow)
        {
            foreach (var col in obj.Mappings)
            {
                col.ColLatter = dataRow.Cells(x => x.Value.ToString() == col.ColName)
                    .FirstOrDefault()?.WorksheetColumn()?.ColumnLetter() ?? string.Empty;
            }
        }

        private static T CreateObjectFromRow<T>(ImportExcelDto obj, IXLRow dataRow)
        {
            var objItem = Activator.CreateInstance<T>();


            MapOtherProperties(objItem, obj, dataRow);


            return objItem;
        }

        private static readonly string[] sourceArray = ["Product", "ModelFamilyName", "Model", "Brand", "Plant"];

        //private static void MapOtherProperties<T>(T objItem, ImportExcelDto obj, IXLRow dataRow)
        //{
        //    foreach (var col in obj.Mappings)
        //    {
        //        var cell = dataRow.Cell(col.ColLatter);
        //        if (cell.HasRichText)
        //        {
        //            var richText = cell.GetRichText();

        //            // Build the HTML representation
        //            StringBuilder htmlBuilder = new StringBuilder();
        //            htmlBuilder.Append("<div>");

        //            foreach (var rt in richText)
        //            {
        //                // Get the text
        //                string text = rt.Text;
        //                string fontColor;
        //                if (rt.FontColor.ColorType == XLColorType.Color)
        //                {
        //                    fontColor = $"#{rt.FontColor.Color.ToArgb() & 0xFFFFFF:X6}"; // Hex color
        //                }
        //                else if (rt.FontColor.ColorType == XLColorType.Theme)
        //                {
        //                    // Map theme colors manually (e.g., based on Excel's default theme color palette)
        //                    fontColor = MapThemeColorToHex(rt.FontColor.ThemeColor, rt.FontColor.ThemeTint);
        //                }
        //                else
        //                {
        //                    fontColor = "#000000"; // Default black for unknown types
        //                }

        //                // Append text with span and style
        //                htmlBuilder.Append($"<span style=\"color:{fontColor};\">{System.Net.WebUtility.HtmlEncode(text)}</span>");
        //            }

        //            htmlBuilder.Append("</div>");

        //            // Output the HTML
        //            string html = htmlBuilder.ToString();
        //        }
        //        else
        //        {
        //            var value = cell.Value.ToString();
        //            var property = objItem.GetType().GetProperty(col.Label);

        //            if (property != null && property.PropertyType == typeof(decimal?))
        //            {
        //                // Try to convert the string value to a decimal?
        //                decimal? convertedValue = null;
        //                if (decimal.TryParse(value, out var decimalValue))
        //                {
        //                    convertedValue = decimalValue;
        //                }
        //                property.SetValue(objItem, convertedValue);
        //            }
        //            else
        //            {
        //                // Handle other property types as needed
        //                property?.SetValue(objItem, value);
        //            }
        //        }
        //    }
        //}
        private static void MapOtherProperties<T>(T objItem, ImportExcelDto obj, IXLRow dataRow)
        {
            foreach (var col in obj.Mappings)
            {
                var cell = dataRow.Cell(col.ColLatter);

                if (cell.HasRichText)
                {
                    // Convert rich text to HTML
                    string html = ConvertRichTextToHtml(cell.GetRichText());
                    MapCellValueToProperty(objItem, col.Label, html);
                    // Process the HTML as needed (assign, save, or log it)
                    // Example: Assign to a specific property of `objItem` if required
                    if (col.ColName == "Name")
                    {
                        var property = objItem.GetType().GetProperty("PlainTextName");
                        if (property != null)
                        {
                            property?.SetValue(objItem, cell.Value.ToString());
                        }
                    }
                }
                else
                {
                    MapCellValueToProperty(objItem, col.Label, cell.Value.ToString());
                }
            }
        }
        private static string ConvertRichTextToHtml(IXLRichText richText)
        {
            StringBuilder htmlBuilder = new StringBuilder("<div>");

            foreach (var rt in richText)
            {
                string text = rt.Text;
                string fontColor = GetFontColor(rt.FontColor);
                htmlBuilder.Append($"<span style=\"color:{fontColor};\">{System.Net.WebUtility.HtmlEncode(text)}</span>");
            }

            htmlBuilder.Append("</div>");
            return htmlBuilder.ToString();
        }
        private static string GetFontColor(XLColor fontColor)
        {
            return fontColor.ColorType switch
            {
                XLColorType.Color => $"#{fontColor.Color.ToArgb() & 0xFFFFFF:X6}",
                XLColorType.Theme => MapThemeColorToHex(fontColor.ThemeColor, fontColor.ThemeTint),
                _ => "#000000" // Default black
            };
        }
        private static void MapCellValueToProperty<T>(T objItem, string propertyName, string value)
        {
            var property = objItem.GetType().GetProperty(propertyName);

            if (property != null)
            {
                object convertedValue = property.PropertyType switch
                {
                    Type t when t == typeof(decimal?) => decimal.TryParse(value, out var decimalValue) ? decimalValue : (decimal?)null,
                    Type t when t == typeof(int?) => int.TryParse(value, out var intValue) ? intValue : (int?)null,
                    _ => value // Default to string or other types
                };

                property.SetValue(objItem, convertedValue);
            }
        }
        private static string MapThemeColorToHex(XLThemeColor themeColor, double tint)
        {
            // Approximate RGB values for default theme colors in Excel
            var themeColorMap = new Dictionary<XLThemeColor, string>
    {
        { XLThemeColor.Text1, "#000000" },
        { XLThemeColor.Background1, "#FFFFFF" },
        { XLThemeColor.Text2, "#1F497D" },
        { XLThemeColor.Background2, "#EEECE1" },
        { XLThemeColor.Accent1, "#4F81BD" },
        { XLThemeColor.Accent2, "#C0504D" },
        { XLThemeColor.Accent3, "#9BBB59" },
        { XLThemeColor.Accent4, "#8064A2" },
        { XLThemeColor.Accent5, "#4BACC6" },
        { XLThemeColor.Accent6, "#F79646" },
        // Add more as needed
    };

            if (themeColorMap.TryGetValue(themeColor, out string baseColor))
            {
                // Apply tint transformation if needed
                return ApplyTintToHex(baseColor, tint);
            }

            return "#000000"; // Fallback to black if not found
        }

        private static string ApplyTintToHex(string baseColor, double tint)
        {
            // Convert hex to RGB
            int r = Convert.ToInt32(baseColor.Substring(1, 2), 16);
            int g = Convert.ToInt32(baseColor.Substring(3, 2), 16);
            int b = Convert.ToInt32(baseColor.Substring(5, 2), 16);

            // Apply tint transformation (simplified)
            if (tint > 0)
            {
                r = (int)(r + (255 - r) * tint);
                g = (int)(g + (255 - g) * tint);
                b = (int)(b + (255 - b) * tint);
            }
            else
            {
                r = (int)(r * (1 + tint));
                g = (int)(g * (1 + tint));
                b = (int)(b * (1 + tint));
            }

            // Clamp values and return new hex
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);

            return $"#{r:X2}{g:X2}{b:X2}";
        }

        private static void DeleteUploadedFile(string fileUploadBasePath, string fileFolderName, string fileName)
        {
            string filePath = Path.Combine(fileUploadBasePath, fileFolderName, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception)
                {
                    // Log or handle exception if necessary
                }
            }
        }


        public static int GetColumnIndex(IXLRow row, string colName)
        {
            int index = 0;
            foreach (var cell in row.Cells())
            {
                index++;
                if (cell.Value.ToString().Equals(colName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return index;
                }
            }
            return -1;
        }
        public static void AddHeader(IXLWorksheet workSheet, List<string> headers)
        {
            int col = 1;
            foreach (string header in headers)
            {
                workSheet.Cell(1, col).Value = header;
                col++;
            }
            // Style the header row
            IXLRow headerRow = workSheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Height = 30;
        }
        public static Stream GenerateExcel(XLWorkbook excel)
        {
            System.IO.Stream spreadsheetStream = new System.IO.MemoryStream();
            excel.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;
            return spreadsheetStream;
        }
        public static class ImportPageEnum
        {
            public static readonly string Products = "Product";
        }

    }
}
