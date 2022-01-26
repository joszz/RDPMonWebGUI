using System.Diagnostics;

namespace RDPMonWebGUI.Middleware;

public class Timer
{
    private readonly RequestDelegate _next;

    public Timer(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Starts a Stopwatch for every request to measure page generation time.
    /// Add the Stopwatch to httpContext.Items, to be retrieved later in the footer, to display the time.
    /// </summary>
    /// <param name="httpContext">The current request's context.</param>
    /// <returns>void</returns>
    public async Task Invoke(HttpContext httpContext)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        httpContext.Items["stopwatch"] = stopwatch;
        await _next(httpContext);
    }
}