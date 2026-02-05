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
			ViewBag.TraceId = HttpContext.TraceIdentifier;

			ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString() ?? feature?.Error?.Message;
			ViewBag.ErrorDetail = TempData["ErrorDetail"]?.ToString();

			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
				ViewBag.StackTrace = feature?.Error?.StackTrace;

			return View("Error");
		}

		[Route("Error/403")]
		public IActionResult AccessDenied()
		{
			ViewBag.StatusCode = 403;
			ViewBag.TraceId = HttpContext.TraceIdentifier;

			ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString() ?? "Erişim izniniz yok";
			ViewBag.ErrorDetail = TempData["ErrorDetail"]?.ToString();

			return View("Error");
		}

		[Route("Error/{statusCode:int}")]
		public IActionResult StatusCodePage(int statusCode)
		{
			ViewBag.StatusCode = statusCode;
			ViewBag.TraceId = HttpContext.TraceIdentifier;

			ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString();
			ViewBag.ErrorDetail = TempData["ErrorDetail"]?.ToString();

			return View("Error");
		}
	}
}
