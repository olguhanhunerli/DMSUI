using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.CAPA;
using DMSUI.Entities.DTOs.Category;
using DMSUI.Entities.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public async Task<CAPADTO> CreateCAPAAsync(CAPACreateReqDTO dto)
        {
            AttachToken();

            using var response = await _httpClient.PostAsJsonAsync("api/CAPA", dto);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"CAPA create failed: {(int)response.StatusCode} {response.ReasonPhrase}. Body: {body}");

            if (string.IsNullOrWhiteSpace(body))
                throw new Exception("CAPA create succeeded but response body was empty.");

            var created = System.Text.Json.JsonSerializer.Deserialize<CAPADTO>(
                body,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (created == null)
            {
                throw new Exception($"CAPA create response could not be parsed. Body: {body}");
            }

            return created;
        }

        public async Task<bool> UpdateCAPAAsync(string capaNo, CAPAUpdateReqDTO dto)
        {
            AttachToken();
            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var request = new HttpRequestMessage(new HttpMethod("PATCH"),
                $"/api/CAPA/{Uri.EscapeDataString(capaNo)}")
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
            Console.WriteLine("PATCH JSON => " + json);
            var response = await _httpClient.SendAsync(request);

            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine("PATCH STATUS: " + (int)response.StatusCode);
            Console.WriteLine("PATCH BODY: " + body);

            return response.IsSuccessStatusCode;

        }
    }
}
