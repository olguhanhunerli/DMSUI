using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Exceptions
{
    public class ApiException: Exception
    {
		public int StatusCode { get; }
		public string? ApiMessage { get; }
		public string? Detail { get; }

		public ApiException(int statusCode, string? apiMessage = null, string? detail = null)
			: base(apiMessage ?? $"API Error (HTTP {statusCode})")
		{
			StatusCode = statusCode;
			ApiMessage = apiMessage;
			Detail = detail;
		}
	}
}
