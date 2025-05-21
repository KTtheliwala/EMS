using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Configuration;
using TheTecniQ.Api.Models.Images;
using System.IO;

namespace TheTecniQ.Api.Controllers.Common
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class ImageController : BaseController
    {
        private readonly string _fileUploadBasePath = AppConfig.FileUploadBasePath;



        [HttpGet("get-image/{imagePath?}")]
        public async Task<IActionResult> GetFileImageNew(string imagePath)
        {
            // Validate imageName
            if (string.IsNullOrEmpty(imagePath))
            {
                return BadRequest(new ApiResponse() { StatusCode = 400, StatusText = "error", Message = "Image name is required." });
            }            

            // Convert enum to folder name string
            string filePath = Uri.UnescapeDataString(Path.Combine(_fileUploadBasePath, imagePath));

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new ApiResponse() { StatusCode = 404, StatusText = "error", Message = "Image not found." });
            }

            try
            {
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "image/jpeg"); // Change the MIME type as per your image type
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse() { StatusCode = 500, StatusText = "error", Message = ex.Message });
            }
        }

        [HttpGet("get-image")]
        public async Task<IActionResult> GetFileImage(ImageEnum mode, string imageName)
        {
            // Validate imageName
            if (string.IsNullOrEmpty(imageName))
            {
                return BadRequest(new ApiResponse() { StatusCode = 400, StatusText = "error", Message = "Image name is required." });
            }

            // Check if the mode is valid
            if (!Enum.IsDefined(typeof(ImageEnum), mode))
            {
                return BadRequest(new ApiResponse() { StatusCode = 400, StatusText = "error", Message = "Invalid mode value." });
            }

            // Convert enum to folder name string
            string folderName = mode.ToString();
            string filePath = Path.Combine(_fileUploadBasePath, folderName, imageName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new ApiResponse() { StatusCode = 404, StatusText = "error", Message = "Image not found." });
            }

            try
            {
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "image/jpeg"); // Change the MIME type as per your image type
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse() { StatusCode = 500, StatusText = "error", Message = ex.Message });
            }
        }

    }
}
