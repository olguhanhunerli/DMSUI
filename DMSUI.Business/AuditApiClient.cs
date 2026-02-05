using DMSUI.Business.Exceptions;
using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Audit;
using DMSUI.Entities.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class AuditApiClient : IAuditApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        private void AttachToken()
        {
            var token = _httpContextAccessor.HttpContext?
                .Request.Cookies["access_token"];

            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task<PagedResultDTO<DocumentAccessLogDTO>> GetDocumentAccessLogsAsync(int page, int pageSize)
        {
            AttachToken();

            var response = await _httpClient.GetAsync(
                $"api/Audit/document-access-logs?page={page}&pageSize={pageSize}"
            );

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new ForbiddenException();   

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PagedResultDTO<DocumentAccessLogDTO>>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new PagedResultDTO<DocumentAccessLogDTO>();
        }
    }
}
