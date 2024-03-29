﻿namespace RDPMonWebGUI.Controllers;

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
        return View(false);
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
            return Redirect("~/");
        }

        ViewData["Title"] = "Login";
        return View(true);
    }

    /// <summary>
    /// Will clear the session, effectively login you out.
    /// </summary>
    /// <returns>Redirects back to login page.</returns>
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index");
    }

    /// <summary>
    /// Checks the provided password against the hashed and salted password in the appsettings.
    /// </summary>
    /// <param name="password">The user provided password to check.</param>
    /// <returns>Whether or not the password was verified successfully</returns>
    private bool VerifyPasswordHash(string password)
    {
        if (_configuration["Password"] != null && _configuration["PasswordSalt"] != null && password != null)
        {
            byte[] salt = Convert.FromBase64String(_configuration["PasswordSalt"]);
            byte[] passwordHash = Convert.FromBase64String(_configuration["Password"]);

            HMACSHA512 hMACSHA512 = new(salt);
            using (var hmac = hMACSHA512)
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

    /// <summary>
    /// Shows the manifest JSON file used by PWA installs.
    /// Sets the correct content-type for the request.
    /// </summary>
    /// <returns>A view with the manifest JSON content</returns>
    public IActionResult Manifest()
    {
        HttpContext.Response.Headers["Content-Type"] = "application/manifest+json; charset=UTF-8";
        return View();
    }

    /// <summary>
    /// Returns the contents of the minified stub service worker. Stubbing the worker to make this project installable as PWA.
    /// Sets the service-worker-allowed header to the parent, so it has the scope of the whole site.
    /// </summary>
    /// <returns>The contents of the minified service worker.</returns>
    public IActionResult ServiceWorker()
    {
        HttpContext.Response.Headers["Service-Worker-allowed"] = "../";
        return File("/js/worker.min.js", "text/javascript");
    }
}