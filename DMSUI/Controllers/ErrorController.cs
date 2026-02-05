using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Index()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.Path = feature?.Path;
            ViewBag.ErrorMessage = feature?.Error?.Message;

            return View("Error");
        }

        [Route("Error/403")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        [Route("Error/{statusCode:int}")]
        public IActionResult StatusCodePage(int statusCode)
        {
            if (statusCode == 403)
                return RedirectToAction(nameof(AccessDenied));

            ViewBag.StatusCode = statusCode;
            return View("Error");
        }
    }
}
