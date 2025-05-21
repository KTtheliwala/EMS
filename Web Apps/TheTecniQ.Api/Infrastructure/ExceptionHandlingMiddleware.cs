using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.API.Infrastructure.Extensions;
using Microsoft.IO;
using DocumentFormat.OpenXml.Bibliography;
using Serilog.Context;
using System.Net.Http;
using Azure.Core;
using System.Linq;
//using TheTecniQ.Services.Logging;

namespace TheTecniQ.API.Infrastructure
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();
        private readonly IConfiguration Configuration = configuration;
        private static readonly string[] separator = [",\"Password\":", ","];

        public async Task Invoke(HttpContext context) //, ILogService _logService, IRequestLogService _requestlog
        {
            //Need to add ExcludeStatusCode login too - in cas we need to ignore validaton error e.g RecordNotFound = 601,
            string IsAllowLog = Configuration["RequestLog:IsAllow"];
            RequestResponseLog log = new();

            LogContext.PushProperty("LogSourceId", (int)EnumLogSource.API);
            try
            {
                if ((IsAllowLog ?? "false").Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    log = await LogRequest(context, log);
                    log = await LogResponse(context, log);
                }
                else
                {
                    context.Request.EnableBuffering();
                    await _next.Invoke(context);
                }
            }
            catch (Exception ex)
            {
                HttpResponse response = context.Response;
                response.ContentType = "application/json";
                log.ResponseCode = "500";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                string param = "";
                if (context.Request.Method.Equals("get", StringComparison.CurrentCultureIgnoreCase))
                {
                    param = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "";
                }
                else
                {
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    using (StreamReader stream = new(context.Request.Body))
                    {
                        param = stream.ReadToEnd();
                    }
                    context.Request.Body.Dispose();
                }
                string msg = ex == null ? "{Params:" + param + "}" : "{ Params: " + param + ",DetailError: " + (ex.InnerException != null ? (ex.InnerException.ToString().Equals("undefined", global::System.StringComparison.OrdinalIgnoreCase) ? ex.Message : ex.InnerException) : ex?.Message ?? "") + "}";
                //string msg = ex == null ? "{Params:" + param + "}" : "{ Params: " + param + ",DetailError: " + ((ex.InnerException != null || ex.InnerException.ToString().ToLower() == "undefined") ? ex.Message : ex.InnerException) + "}            ";
                //await _logService.ErrorAsync($"{context.Request.Method}-{context.Request.Path.Value}", msg, Core.Domain.Logging.LogSource.API, LogType.Error, ex, 0);
                string errorJson = JsonSerializer.Serialize("Error. Please try again after some time.".ToMessage(ApiStatusCode.Status500InternalServerError, new { ApiUrl = context.Request.Path.Value }));
                int UserId = (context?.User?.Claims?.FirstOrDefault(c => c.Type == "Id")?.Value?.Descrypt() ?? "0").ToInt();
                string Username = context?.User?.Claims?.FirstOrDefault(c => c.Type == "UserName")?.Value?.Descrypt() ?? "";
                LogContext.PushProperty("LogTypeId", (int)EnumLogType.Error);
                LogContext.PushProperty("UserId", UserId);
                LogContext.PushProperty("Username", Username);
                Serilog.Log.Error(ex, ("Error By Call =>" + context.Request.Path + ",Params:" + param));
                Serilog.Log.CloseAndFlush();

                await response.WriteAsync(errorJson);
            }
            finally
            {
                //Informational responses (100–199),Successful responses (200–299),Redirection messages (300–399),Client error responses (400–499),Server error responses (500–599)
                if ((!log.Response.Contains("\"StatusCode\":200") && log.Response.Contains("StatusCode") && context.Request.Method.Equals("post", StringComparison.OrdinalIgnoreCase)) || context.Response.StatusCode < (int)ApiStatusCode.Status200OK || context.Response.StatusCode > 299)
                {
                    //await _requestlog.InsertAsync(log);
                }
            }
        }
        private async Task<RequestResponseLog> LogRequest(HttpContext context, RequestResponseLog Log)
        {
            context.Request.EnableBuffering();
            await using MemoryStream requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            Log.Path = context.Request.Path;
            Log.Method = context.Request.Method;
            Log.QueryString = context.Request.QueryString.ToString();
            Log.RequestedOn = DateTime.UtcNow;
            if (context.Request.Method != "GET")
            {
                Log.Payload = ReadStreamInChunks(requestStream);
                if (context.Request.Path.ToString().ToLower().EndsWith("/authenticate"))
                {
                    var pass = Log.Payload.Split(separator, StringSplitOptions.None);
                    if (pass.Length > 1)
                        Log.Payload = Log.Payload.Replace(pass[1], "\"DummayPassword\"");
                }
            }
            context.Request.Body.Position = 0;
            return Log;
        }
        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using StringWriter textWriter = new();
            using StreamReader reader = new(stream);
            char[] readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                0,
                                                readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }
        private async Task<RequestResponseLog> LogResponse(HttpContext context, RequestResponseLog Log)
        {
            Stream originalBodyStream = context.Response.Body;
            await using MemoryStream responseBody = _recyclableMemoryStreamManager.GetStream();
            try
            {
                context.Response.Body = responseBody;
                await _next(context);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                Log.Response = text;
            }
            catch (Exception ex)
            {
                Log.Response = (ex?.Message ?? "") + ";Inner=" + (ex?.InnerException?.Message ?? "");
            }
            Log.RespondedOn = DateTime.UtcNow;
            Log.ResponseCode = context.Response.StatusCode.ToString();
            await responseBody.CopyToAsync(originalBodyStream);
            return Log;
        }
    }
}
