using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RDPMonWebGUI.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace RDPMonWebGUI.Controllers
{
    public class SessionController : BaseController
    {
        public SessionController(LiteDbContext context, IConfiguration configuration) : base(context, configuration) { }

        /// <summary>
        /// Show the login view.
        /// </summary>
        /// <returns>The login view.</returns>
        public IActionResult Index()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        /// <summary>
        /// Handles the form submitted from the login view.
        /// Checks the password and if mathces, redirects to the homepage.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string password)
        {
            if (VerifyPasswordHash(password))
            {
                HttpContext.Session.SetString("loggedin", "true");
                return RedirectToAction("Index", "Home");
            }

            ViewData["Title"] = "Login";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

        private bool VerifyPasswordHash(string password)
        {
            if (_configuration["Password"] != null && _configuration["PasswordSalt"] != null)
            {
                byte[] salt = Convert.FromBase64String(_configuration["PasswordSalt"]);
                byte[] passwordHash = Convert.FromBase64String(_configuration["Password"]);

                using (var hmac = new HMACSHA512(salt))
                {
                    byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != passwordHash[i])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }
}
