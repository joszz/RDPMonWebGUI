namespace RDPMonWebGUI.Controllers;

public class ErrorController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(int code = 404) => View(new ErrorViewModel
    {
        StatusCode = code
    });

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Exception() =>
        View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
}
