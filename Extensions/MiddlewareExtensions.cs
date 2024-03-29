﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RDPMonWebGUI.Middleware;

namespace RDPMonWebGUI.Extensions;

public static class MiddlewareExtensions
{
    /// <summary>
    /// An extension method for IServiceCollection to setup LiteDB for dependency injection.
    /// </summary>
    /// <param name="services">The service collection injected in the Startup class.</param>
    /// <param name="databasePath">The path to the database to use.</param>
    public static void AddLiteDb(this IServiceCollection services, string databasePath)
    {
        services.AddTransient<LiteDbContext, LiteDbContext>();
        services.Configure<LiteDbConfig>(options => options.DatabasePath = databasePath);
    }

    /// <summary>
    /// An extension method for IApplicationBuilder to inject headers related to security aspects.
    /// </summary>
    /// <param name="builder">The applicationbuilder injected in the startup class.</param>
    /// <returns>IApplicationBuilder to chain calls.</returns>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder) =>
        builder.UseMiddleware<SecurityHeaders>();

    /// <summary>
    /// An extension method for IApplicationBuilder to start a stopwatch to measure entire page generation time.
    /// </summary>
    /// <param name="builder">The applicationbuilder injected in the startup class.</param>
    /// <returns>IApplicationBuilder to chain calls.</returns>
    public static IApplicationBuilder UseTimer(this IApplicationBuilder builder) =>
        builder.UseMiddleware<Timer>();
}