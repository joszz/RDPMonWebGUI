using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RDPMonWebGUI.Extensions;
using RDPMonWebGUI.Filters;
using System;

namespace RDPMonWebGUI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.Secure = CookieSecurePolicy.Always;
            });
            services.Configure<RouteOptions>(options => options.AppendTrailingSlash = true);

            services.AddLiteDb(_configuration.GetConnectionString("RDPMon"));
            services.AddAntiforgery();
            services.AddMemoryCache();
            services.AddHsts(options =>
            {
                options.Preload = options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            services.AddControllersWithViews(config =>
            {
                AuthenticationFilter filter = new(_configuration, _environment);
                config.Filters.Add(filter);
            }).AddRazorRuntimeCompilation();
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

            services.AddDataProtection();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseTimer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Exception");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
            app.UseSession();
            app.UseSecurityHeaders();

            FileExtensionContentTypeProvider contentTypeProvider = new();
            StaticFileOptions staticFileOptions = new()
            {
                ContentTypeProvider = contentTypeProvider,
                OnPrepareResponse = ctx =>
                {
                    //Cache static files for a week
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = "public,max-age=604800";
                }
            };
            app.UseStaticFiles(staticFileOptions);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{SortField?}/{SortDirection?}");
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "~/",
                    new { Controller = "Home", Action = "Index" }
                );
            });
        }
    }
}
