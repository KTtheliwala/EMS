using Microsoft.AspNetCore.Builder;
using TheTecniQ.Core.Configuration;
using TheTecniQ.Core.Infrastructure;
using Serilog;

namespace TheTecniQ.API.Infrastructure.Extensions;

/// <summary>
/// Represents extensions of IApplicationBuilder
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configure the application HTTP request pipeline
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void ConfigureRequestPipeline(this IApplicationBuilder application)
    {
        //Load service resolver
        ServiceResolver.Configure(application);

        //app.UseResponseCompression();
        application.UseSwagger();
        application.UseSwaggerUI(c => c.SwaggerEndpoint(AppConfig.SwaggerUrl, "API v1"));
        application.UseHttpsRedirection();
        application.UseAuthentication();
        application.UseMiddleware<ExceptionHandlingMiddleware>();
        application.UseRouting();
        application.UseCors("apicorspolicy"); //if (!app.Environment.IsDevelopment()){app.UseCors("corspolicy");}else{app.UseCors("dev_corspolicy");}
        application.UseAuthorization();
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        application.UseSerilogRequestLogging();
    }
}