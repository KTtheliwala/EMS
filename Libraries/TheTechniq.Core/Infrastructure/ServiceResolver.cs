using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace TheTecniQ.Core.Infrastructure;

/// <summary>
/// Represents service resolver
/// </summary>
public static class ServiceResolver
{
    #region Utilities

    /// <summary>
    /// Get IServiceProvider
    /// </summary>
    /// <returns>IServiceProvider</returns>
    public static IServiceProvider GetServiceProvider(IServiceScope scope = null)
    {
        if (scope == null)
        {
            var accessor = ServiceProvider?.GetService<IHttpContextAccessor>();
            var context = accessor?.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }
        return scope.ServiceProvider;
    }

    /*protected virtual Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        var assemblyFullName = args.Name;

        //check for assembly already loaded
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyFullName);
        if (assembly != null)
            return assembly;

        //get assembly from TypeFinder
        var typeFinder = Singleton<ITypeFinder>.Instance;

        return typeFinder?.GetAssemblyByName(assemblyFullName);
    }*/

    #endregion

    #region Methods

    /// <summary>
    /// Configure HTTP request pipeline
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void Configure(IApplicationBuilder application)
    {
        ServiceProvider = application.ApplicationServices;
    }

    /// <summary>
    /// Configure HTTP request pipeline
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void Configure(IServiceProvider services)
    {
        ServiceProvider = services;
    }

    /// <summary>
    /// Resolve dependency
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <typeparam name="T">Type of resolved service</typeparam>
    /// <returns>Resolved service</returns>
    public static T Resolve<T>(IServiceScope scope = null) where T : class
    {
        return (T)Resolve(typeof(T), scope);
    }

    /// <summary>
    /// Resolve dependency
    /// </summary>
    /// <param name="type">Type of resolved service</param>
    /// <param name="scope">Scope</param>
    /// <returns>Resolved service</returns>
    public static object Resolve(Type type, IServiceScope scope = null)
    {
        return GetServiceProvider(scope)?.GetService(type);
    }

    /// <summary>
    /// Resolve dependencies
    /// </summary>
    /// <typeparam name="T">Type of resolved services</typeparam>
    /// <returns>Collection of resolved services</returns>
    public static IEnumerable<T> ResolveAll<T>()
    {
        return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
    }

    /// <summary>
    /// Resolve unregistered service
    /// </summary>
    /// <param name="type">Type of service</param>
    /// <returns>Resolved service</returns>
    public static object ResolveUnregistered(Type type)
    {
        Exception innerException = null;
        foreach (var constructor in type.GetConstructors())
            try
            {
                //try to resolve constructor parameters
                var parameters = constructor.GetParameters().Select(parameter =>
                {
                    var service = Resolve(parameter.ParameterType) ?? throw new Exception("Unknown dependency");
                    return service;
                });

                //all is ok, so create instance
                return Activator.CreateInstance(type, parameters.ToArray());
            }
            catch (Exception ex)
            {
                innerException = ex;
            }

        throw new Exception("No constructor was found that had all the dependencies satisfied.", innerException);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Service provider
    /// </summary>
    public static IServiceProvider ServiceProvider 
    {
        get
        {
            if (Singleton<IServiceProvider>.Instance != null)
            {
                return Singleton<IServiceProvider>.Instance;
            }
            return null;
        }
        set
        {
            Singleton<IServiceProvider>.Instance = value;
        }
    }

    #endregion
}