using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest)
        {
            var json = JsonSerializer.Serialize(new
            {
                email = loginRequest.Email,
                password = loginRequest.Password
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");   
            var response = await _httpClient.PostAsync("api/Auth/login", content);

            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var resultJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponseDTO>(
                resultJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        }

        public async Task<LoginResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            var json = JsonSerializer.Serialize(refreshToken);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Auth/refresh-token", content);

            if(!response.IsSuccessStatusCode)
            {
                return null;
            }

            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponseDTO>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
    }
}
