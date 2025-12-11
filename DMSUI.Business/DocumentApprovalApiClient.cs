using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Approval;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Document;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class DocumentApprovalApiClient : IDocumentApprovalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentApprovalApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<List<MyPendingDocumentDTO>> GetMyPendingApprovalAsync(int page, int pageSize)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Document/my-pending-approvals/paged?page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();

            var pagedResult =
                await response.Content.ReadFromJsonAsync<PagedResultDTO<MyPendingDocumentDTO>>();

            return pagedResult.Items.Select(x => new MyPendingDocumentDTO
            {
                DocumentCode = x.DocumentCode,
                Title = x.Title,
                StatusId = x.StatusId,
                CreatedAt = x.CreatedAt
            }).ToList();
        }

        public async Task InitApprovalAsync(CreateDocumentApprovalDTO createDocumentApprovalDTO)
        {
            AttachToken();
            var response = await _httpClient.GetAsync("api/DocumentApproval/init-approval");
            response.EnsureSuccessStatusCode();
        }

        public async Task ApproveAsync(int documentId)
        {
            AttachToken();
            var response = await _httpClient.PostAsync($"api/DocumentApproval/approve?documentId={documentId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RejectAsync(int documentId, string reason)
        {
            AttachToken();
            var response = await _httpClient.PostAsync($"api/DocumentApproval/reject?documentId={documentId}&reason={reason}",null);
            response.EnsureSuccessStatusCode();
        }
    }
}
