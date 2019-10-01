using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RDPMonWebGUI.Middleware;
using RDPMonWebGUI.Models;

namespace RDPMonWebGUI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void AddLiteDb(this IServiceCollection services, string databasePath)
        {
            services.AddTransient<LiteDbContext, LiteDbContext>();
            services.Configure<LiteDbConfig>(options => options.DatabasePath = databasePath);
        }

        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder) =>
            builder.UseMiddleware<SecurityHeaders>();
    }
}
