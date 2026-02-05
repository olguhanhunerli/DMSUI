using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Business.Exceptions
{
    public static class HttpResponseExtensions
    {
        private static readonly JsonSerializerOptions JsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenException();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();

            response.EnsureSuccessStatusCode();

            if (response.Content == null)
                return default!;

            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
                return default!;

            return JsonSerializer.Deserialize<T>(body, JsonOptions)!;
        }
        public static async Task<bool> EnsureSuccessOrThrowAsync(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenException();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();

            return response.IsSuccessStatusCode;
        }
        public static void ThrowIfUnauthorizedOrForbidden(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenException();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();
        }
    }
}
