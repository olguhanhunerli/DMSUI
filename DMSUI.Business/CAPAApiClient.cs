using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.CAPA;
using DMSUI.Entities.DTOs.Category;
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
    public class CAPAApiClient : ICAPAApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CAPAApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<PagedResultDTO<CAPADTO>> GetAllCAPAS(int page, int pageSize)
        {
            AttachToken();
            var result = await _httpClient.GetAsync($"/api/CAPA/GetAll?page={page}&pageSize={pageSize}");
            if (result == null)
            {
                return new PagedResultDTO<CAPADTO>();
            }
            var body = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResultDTO<CAPADTO>>(
               body,
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
           ) ?? new PagedResultDTO<CAPADTO>();
        }

        public async Task<CAPADetailDTO> GetCAPASByCapaNo(string capaNo)
        {
            AttachToken();
            var result = await _httpClient.GetAsync($"/api/CAPA/GetByCapaNo?capaNo={capaNo}");
            if (result == null)
            {
                return new CAPADetailDTO();
            }
            var body = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CAPADetailDTO> (
               body,
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
           );

        }

        public async Task<CAPACreateFormDTO> CreateFormCAPAS(string complaintNo)
        {
            AttachToken();
            var result = await _httpClient.GetAsync($"/api/CAPA/create-form?complaintNo={complaintNo}");
            if(result == null)
                return new CAPACreateFormDTO();
            var body = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CAPACreateFormDTO>(
                body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
