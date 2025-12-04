using DMSUI.Entities.DTOs.Auth;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.ApiAuthMessageHandlers
{
    public class ApiAuthMessageHandler: DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ApiAuthMessageHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request,
           CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;
            var accessToken = context?.Request.Cookies["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;


            var refreshToken = context?.Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return response;

            var apiBaseUrl = _configuration["APISettings:BaseUrl"];

            using var refreshClient = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl)
            };

            var refreshContent = new StringContent(
                JsonSerializer.Serialize(refreshToken),
                Encoding.UTF8,
                "application/json"
            );

            var refreshResponse = await refreshClient
                .PostAsync("/api/Auth/refresh-token", refreshContent);

            if (!refreshResponse.IsSuccessStatusCode)
                return response;

            var json = await refreshResponse.Content.ReadAsStringAsync();

            var newTokens = JsonSerializer.Deserialize<LoginResponseDTO>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            context!.Response.Cookies.Append("access_token", newTokens.AccessToken);
            context.Response.Cookies.Append("refresh_token", newTokens.RefreshToken);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", newTokens.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
