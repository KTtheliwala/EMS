using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using Asp.Versioning;
using TheTecniQ.API.Controllers;
using TheTecniQ.API.Models.Common;
using TheTecniQ.API.Models.Users;
using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Api.Infrastructure;
using System.Linq;
using TheTecniQ.Api.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Permissions;
using TheTecniQ.Services.Common;

using TheTecniQ.API.Models.Masters;
using TheTecniQ.Core.Configuration;
using DocumentFormat.OpenXml.Wordprocessing;
using System;

namespace TheTecniQ.Api.Controllers.Masters
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class DashboardController() : BaseController
    {
        #region Fields
        
        #endregion

        #region Messages
        private readonly string IsExistMsg = "This Yantra is already exist!";
        private readonly string SaveMsg = "Yantra added successfully.";
        private readonly string UpdateMsg = "Yantra updated successfully.";
        private readonly string DeleteMsg = "Yantra deleted successfully.";
        #endregion

        #region FilePath
        private readonly string FileUploadBasePath = AppConfig.FileUploadBasePath;
        private readonly string FileFolderName = "Yantra";
        #endregion

       
    }
}
