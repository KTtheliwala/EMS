using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using TheTecniQ.Api.Models.Editor;
using static TheTecniQ.Services.ServiceCommonExtensions;
using TheTecniQ.API.Infrastructure.Extensions;

namespace TheTecniQ.Api.Controllers.Editor
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1")]
    public class EditorController : BaseController
    {        
        private readonly IConfiguration _Configuration;

        public EditorController(IConfiguration configuration)
        {           
            _Configuration = configuration;
        }

        [HttpPost]
        //[Permission(Page = PageName.AdmMasDocentTrainingBanner,Role = TokenRole.Admin, Permission = PagePermission.Edit)]
        public async Task<IActionResult> Post(IList<IFormFile> files)
        {
            string baseURL = _Configuration["FrontEndDomainUrl"] + "Upload/";
            string fileName = "";
            string fullfileName = "";
            bool isImages = true;
            List<string> list = new();
            if (files != null && files.Count > 0)
            {
                foreach (IFormFile item in files)
                {
                    IFormFile bannerImageFile = item;
                    if (bannerImageFile.Length > FileUploadConstants.MaxImageSizeBytes)
                    {
                        //return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "Image size should be less than 5 MB.");
                    }

                    fileName = Guid.NewGuid().ToString();
                    string fileExtension = Path.GetExtension(item.FileName);
                    string result = null;

                    if (result == null)
                    {
                        //return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "Only .jpg, .jpeg, .gif, and .png file extensions are allowed.");

                    }
                    fullfileName = fileName + fileExtension;
                    list.Add(Path.Combine(baseURL, fileName));
                }
            }
            return Ok(new
            {
                success = true,
                data = new
                {
                    files = new[] { fileName },
                    baseurl = baseURL,
                    message = (list != null && list.Count > 0) ? "File uploaded Successfully." : "File not uploaded Successfully.",
                    isImages,
                    error = "",
                    path = baseURL + "/" + fullfileName
                }
            });
            //return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, (list != null && list.Count > 0) ? "File uploaded Successfully." : "File not uploaded Successfully.", list);
        }

        [HttpPost]
        [Route("single-fileupload")]
        [Produces("application/json")]
        public async Task<IActionResult> SingleFilePost(IFormFile file)
        {

            IFormFileCollection filess = Request.Form.Files;

            // Get the file from the POST request
            IFormFile theFile = filess.FirstOrDefault();
            string fileRoute = "";

            string fullPath = Path.Combine(fileRoute, theFile.FileName);

            // Create directory if it does not exist.
            FileInfo dir = new(fileRoute);
            if (!dir.Exists)
            {
                dir.Directory.Create();
            }
            /*
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await theFile.CopyToAsync(stream);
            }
            */
            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                await theFile.CopyToAsync(stream);
                stream.Dispose();
            }
            string AppBaseUrl = Path.Combine(_Configuration["FrontEndDomainUrl"] + "Upload/", "");


            return Ok(new
            {
                success = true,
                data = new
                {
                    files = new[] { theFile.FileName },
                    baseurl = $"{AppBaseUrl}/",
                    message = "",
                    error = "",
                    path = $"{AppBaseUrl}/{theFile.FileName}"
                }
            });
        }

        [HttpPost]
        [Route("get-list")]
        public ApiResponse Browse()
        {
            List<object> list = new();
            try
            {
                // Get the absolute path to the uploads folder
                string uploadsPath = Path.Combine(_Configuration["FileUploadPathSettings:FilePath"], "CommonEnum.enmFolder.images.ToDescription()");
                string baseURL = _Configuration["FrontEndDomainUrl"];
                string path = "/Upload/" ;

                // Ensure the uploads folder exists
                if (!Directory.Exists(uploadsPath))
                {
                    //return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "Root doesn't exist", list);
                    return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status400BadRequest, "Root doesn't exist");
                }

                // Get a list of files in the uploads folder
                //list.Add()
                var files = Directory.GetFiles(uploadsPath)
                    .Select(filePath => new
                    {
                        //baseurl = baseURL,
                        //path= path,
                        files = Path.GetFileName(filePath),
                        type = "image"
                    })
                    .ToList();
                list.Add(new
                {
                    baseurl = baseURL,
                    path,
                    files,
                    name = "default"
                });
                //return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "File uploaded list.", list);
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status200OK, "File uploaded list");
            }
            catch (Exception)
            {
                return APIResponseExtensions.GenerateResponse(ApiStatusCode.Status500InternalServerError, "Internal Server Error.");
            }
        }

        [HttpPost]
        [Route("permission")]
        public IActionResult Permission([FromForm] EditorModel model)
        {
            try
            {
                if (model.action == "permissions")
                {
                    return Ok(new
                    {
                        success = true,
                        time = DateTime.Now,
                        data = new
                        {
                            permissions = new
                            {
                                allowFiles = true,
                                allowFileMove = true,
                                allowFileUpload = true,
                                allowFileUploadRemote = true,
                                allowFileRemove = false,
                                allowFileRename = true,
                                allowFolders = false,
                                allowFolderMove = false,
                                allowFolderCreate = false,
                                allowFolderRemove = false,
                                allowFolderRename = false,
                                allowImageResize = false,
                                allowImageCrop = false
                            }
                        }
                    });
                }
                if (model.action == "files")
                {
                    List<object> list = new();
                    string uploadsPath = "";
                    string baseURL = _Configuration["FrontEndDomainUrl"];
                    string pathExt = "/Upload/";

                    var files = Directory.GetFiles(uploadsPath)
                        .Select(filePath => new
                        {
                            //baseurl = baseURL,
                            //path= path,
                            file = Path.GetFileName(filePath),
                            thumb = Path.GetFileName(filePath),
                            changed = DateTime.Now,
                        })
                        .ToList();
                    list.Add(new
                    {
                        baseurl = baseURL,
                        path = pathExt,
                        files,
                        code = 220
                    });

                    return Ok(new
                    {
                        success = true,
                        time = DateTime.Now,
                        data = new
                        {
                            sources = list
                        }
                    });
                }
                if (model.action == "folders")
                {
                    List<object> list = new();
                    string uploadsPath = "";
                    string baseURL = _Configuration["FrontEndDomainUrl"];
                    string pathExt = "/Upload/" ;

                    var files = Directory.GetFiles(uploadsPath)
                        .Select(filePath => new
                        {
                            //baseurl = baseURL,
                            //path= path,
                            file = Path.GetFileName(filePath),
                            thumb = Path.GetFileName(filePath),
                            changed = DateTime.Now,
                        })
                        .ToList();
                    list.Add(new
                    {
                        baseurl = uploadsPath,
                        path = "",
                        files,
                        code = 220
                    });

                    return Ok(new
                    {
                        success = true,
                        time = DateTime.Now,
                        data = new
                        {
                            sources = list
                        }
                    });
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        
    }
}
