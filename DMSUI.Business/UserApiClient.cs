using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserListDTO>> GetAllUsersAsync()
        {
            var response =await  _httpClient.GetAsync("api/User/GetAllUserInfo");
            if(!response.IsSuccessStatusCode)
            {
                return new List<UserListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<UserListDTO>>(
                body,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<UserListDTO>();

        }

        public async Task<UserListDTO?> GetUserByIdAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/User/GetUserById/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var body = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<UserListDTO>(
                body,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
    }
}
