using Microsoft.AspNetCore.Http;

namespace TheTecniQ.Api.Models.Editor
{
    public class EditorModel
    {
        public IFormFile files { get; set; }
        public string action { get; set; }
        public string path { get; set; }
        public string source { get; set; }
    }
}
