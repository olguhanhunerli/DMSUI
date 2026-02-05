using System.Net;
using System.Text.Json;
using DMSUI.Business.Exceptions;

namespace DMSUI.Business.Exceptions
{
	public static class HttpResponseExtensions
	{
		private static readonly JsonSerializerOptions JsonOptions =
			 new() { PropertyNameCaseInsensitive = true };

		public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				if (response.Content == null) return default!;
				var bodyOk = await response.Content.ReadAsStringAsync();
				if (string.IsNullOrWhiteSpace(bodyOk)) return default!;
				return JsonSerializer.Deserialize<T>(bodyOk, JsonOptions)!;
			}

			await ThrowApiError(response);
			return default!;
		}

		public static async Task<bool> EnsureSuccessOrThrowAsync(this HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
				return true;

			await ThrowApiError(response);
			return false; 
		}
		public static void ThrowIfUnauthorizedOrForbidden(this HttpResponseMessage response)
		{
			if (response.StatusCode == HttpStatusCode.Forbidden)
				throw new ForbiddenException();

			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new UnauthorizedException();
		}
		private static async Task ThrowApiError(HttpResponseMessage response)
		{
			var status = (int)response.StatusCode;

			if (response.StatusCode == HttpStatusCode.Forbidden)
				throw new ForbiddenException();

			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new UnauthorizedException();

			string? body = null;
			if (response.Content != null)
				body = await response.Content.ReadAsStringAsync();

			string? message = null;
			string? detail = null;

			if (!string.IsNullOrWhiteSpace(body))
			{
				try
				{
					var err = JsonSerializer.Deserialize<APIErrorDTO>(body, JsonOptions);
					message = err?.Title ?? err?.Message;
					detail = err?.Detail ?? body;
				}
				catch
				{
					detail = body; 
				}
			}

			message ??= status switch
			{
				400 => "Geçersiz istek (400)",
				404 => "Kayıt bulunamadı (404)",
				409 => "Çakışma / işlem yapılamadı (409)",
				500 => "Sunucu hatası (500)",
				_ => $"Beklenmeyen hata ({status})"
			};

			throw new ApiException(status, message, detail);
		}
	}
}
