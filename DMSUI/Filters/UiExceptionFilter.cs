using DMSUI.Business.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Filters
{
	public class UiExceptionFilter : IExceptionFilter
	{
		private readonly ITempDataDictionaryFactory _tempDataFactory;

		public UiExceptionFilter(ITempDataDictionaryFactory tempDataFactory)
		{
			_tempDataFactory = tempDataFactory;
		}

		public void OnException(ExceptionContext context)
		{
			var tempData = _tempDataFactory.GetTempData(context.HttpContext);

			if (context.Exception is ForbiddenException)
			{
				context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
				context.ExceptionHandled = true;
				return;
			}

			if (context.Exception is UnauthorizedException)
			{
				context.Result = new RedirectToActionResult("Login", "Auth", null);
				context.ExceptionHandled = true;
				return;
			}

			if (context.Exception is ApiException apiEx)
			{
				tempData["ErrorMessage"] = apiEx.ApiMessage;
				tempData["ErrorDetail"] = apiEx.Detail;

				context.Result = new RedirectToActionResult("StatusCodePage", "Error", new { statusCode = apiEx.StatusCode });
				context.ExceptionHandled = true;
				return;
			}

			// Diğer tüm beklenmeyen hatalar
			tempData["ErrorMessage"] = "Beklenmeyen bir hata oluştu.";
			tempData["ErrorDetail"] = context.Exception.Message;

			context.Result = new RedirectToActionResult("Index", "Error", null);
			context.ExceptionHandled = true;
		}
	}
}
