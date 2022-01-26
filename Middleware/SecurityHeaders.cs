namespace RDPMonWebGUI.Middleware;

public class SecurityHeaders
{
    private readonly RequestDelegate _next;

    public SecurityHeaders(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Sets the security related headers for all responses (and als the stupid IE compatibility header, you know, IE sucks...).
    /// </summary>
    /// <param name="httpContext">The current request's context.</param>
    /// <returns>void</returns>
    public async Task Invoke(HttpContext httpContext)
    {
        httpContext.Response.Headers["X-UA-Compatible"] = "IE=edge";
        httpContext.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        httpContext.Response.Headers["X-Content-Type-Options"] = "nosniff";
        httpContext.Response.Headers["X-Frame-Options"] = "sameorigin";
        httpContext.Response.Headers["Referrer-Policy"] = "same-origin";
        httpContext.Response.Headers["Content-Security-Policy"] = "default-src 'self' data:;style-src 'self' 'unsafe-inline'";

        await _next(httpContext);
    }
}
