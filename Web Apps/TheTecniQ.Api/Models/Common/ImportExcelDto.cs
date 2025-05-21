using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TheTecniQ.Api.Models.Common
{
    public class ImportExcelDto
    {
        public IFormFile File { get; set; }
        [JsonIgnore]
        public string FileName { get; set; }
        [JsonIgnore]
        public string ImportPage { get; set; } = "";
        public List<ImportExcelMapping> Mappings { get; set; }

    }
    public class ImportExcelMapping
    {
        public int? Id { get; set; } = 0;
        public string Label { get; set; }
        public string ColName { get; set; }
        [JsonIgnore]
        public string ColLatter { get; set; }
    }
}
