using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RDPMonWebGUI.Controllers;
using System;
using System.Security.Cryptography;
using System.Text;

namespace RDPMonWebGUI.Filters
{
    public class AuthenticationFilter : IActionFilter
    {
        protected readonly IConfiguration _configuration;
        protected readonly IHostEnvironment _environment;

        public AuthenticationFilter(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        /// <summary>
        /// Called before any action is executed, making sure the user has been loggedin if the application requires login.
        /// </summary>
        /// <param name="context">The current context of execution.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            SetPasswordIfChanged();

            bool loginEnabled = !string.IsNullOrEmpty(_configuration["Password"]);
            ((Controller)context.Controller).ViewData["login-enabled"] = loginEnabled;

            if (loginEnabled && string.IsNullOrEmpty(context.HttpContext.Session.GetString("loggedin")) &&
                context.Controller.GetType() != typeof(SessionController) && context.Controller.GetType() != typeof(ErrorController))
            {
                context.Result = ((Controller)context.Controller).RedirectToAction("Index", "Session");
            }
        }

        /// <summary>
        /// Sets the new hash and salted password if there's no PasswordSalt entry in appsettings and the Password entry is available.
        /// </summary>
        private void SetPasswordIfChanged()
        {
            if (!string.IsNullOrEmpty(_configuration["Password"]) && string.IsNullOrEmpty(_configuration["PasswordSalt"]))
            {
                IFileProvider fileProvider = _environment.ContentRootFileProvider;
                IFileInfo fileInfo = fileProvider.GetFileInfo("appsettings.json");

                string physicalPath = fileInfo.PhysicalPath;
                JObject jObject = JsonConvert.DeserializeObject<JObject>(System.IO.File.ReadAllText(physicalPath));

                using (HMACSHA512 hmac = new HMACSHA512())
                {
                    jObject["PasswordSalt"] = Convert.ToBase64String(hmac.Key);
                    jObject["Password"] = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(_configuration["Password"])));
                }

                System.IO.File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
                ((IConfigurationRoot)_configuration).Reload();
            }
        }
    }
}
