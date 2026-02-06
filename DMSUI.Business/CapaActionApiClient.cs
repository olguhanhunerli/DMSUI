using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.CapaActions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class CapaActionApiClient : ICapaActionsApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CapaActionApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<CapaActionDTO> CreateCapaActionAsync(string capaNo, CreateCapaActionDTO dto)
        {
            AttachToken();

            dto.DueDate = DateTime.SpecifyKind(dto.DueDate, DateTimeKind.Local).ToUniversalTime();

            var payload = new
            {
                actionType = dto.ActionType,
                description = dto.Description,
                ownerId = dto.OwnerId,
                dueDate = dto.DueDate.ToString("O"), // ISO 8601 (2026-02-06T13:28:04.1070000Z)
                evidenceRequired = dto.EvidenceRequired
            };

            var json = JsonSerializer.Serialize(payload);

            Console.WriteLine("SENT JSON: " + json);

            using var response = await _httpClient.PostAsync(
                $"/api/CapaActions/{capaNo}/actions",
                new StringContent(json, Encoding.UTF8, "application/json"));

            var raw = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"RESP {(int)response.StatusCode} {response.ReasonPhrase}");
            Console.WriteLine("RESP BODY: " + (string.IsNullOrWhiteSpace(raw) ? "<empty>" : raw));

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(
                    $"API Error {(int)response.StatusCode} {response.ReasonPhrase} | Body: {(string.IsNullOrWhiteSpace(raw) ? "<empty>" : raw)} | Sent: {json}");
            }

            var result = JsonSerializer.Deserialize<CapaActionDTO>(
                raw,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;

        }
    }
}
