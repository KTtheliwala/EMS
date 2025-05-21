using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.Versioning;

namespace TheTecniQ.Api.Infrastructure
{
    public class FileUploadHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string fileUploadBasePath, string folderName)
        {
            string uploadPath = Path.Combine(fileUploadBasePath, folderName);
            Directory.CreateDirectory(uploadPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public static string IsValidFile(string fileName, long fileSize, string[] allowedExtensions, long maxFileSizeKb)
        {
            string rtn = "";
            // Validate file extension
            string extension = Path.GetExtension(fileName)?.ToLower();
            if (string.IsNullOrWhiteSpace(extension) || !allowedExtensions.Contains(extension))
            {
                return rtn = $"Invalid file extension for '{fileName}'.";
            }

            // Validate file size
            if (fileSize > maxFileSizeKb * 1024) // Convert KB to bytes
            {
                return rtn != ""
    ? $"Invalid file extension and file size must be under 5MB for '{fileName}'."
    : $"File size must be under 5MB for '{fileName}'.";
            }

            return rtn;
        }
        [SupportedOSPlatform("windows")]
        public static void ResizeImage(string inputPath, string outputPath, int width, int height)
        {
            using (var image = Image.FromFile(inputPath))
            {
                var resized = new Bitmap(width, height);

                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;

                    graphics.DrawImage(image, 0, 0, width, height);
                }

                resized.Save(outputPath, ImageFormat.Jpeg); // Save in your preferred format (JPEG, PNG, etc.)
            }
        }
        public static async Task<string> GetBase64StringAsync(string fileUploadBasePath, string folderName, string fileName)
        {
            string filePath = Path.Combine(fileUploadBasePath, folderName, fileName);

            if (!File.Exists(filePath))
            {
                return "";
            }

            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            string base64String = Convert.ToBase64String(fileBytes);

            return base64String;
        }
    }
}
