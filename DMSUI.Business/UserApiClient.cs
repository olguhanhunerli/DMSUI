using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Position;
using DMSUI.Entities.DTOs.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DMSUI.Business
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<bool> CreateUserAsync(UserRegisterDTO userRegisterDTO)
        {

          AttachToken();
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", userRegisterDTO);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API ERROR BODY: " + error);
                return false;
            }
            return response.IsSuccessStatusCode;

        }

        public async Task<List<UserListDTO>> GetAllUsersAsync()
        {
			AttachToken();

			var response = await _httpClient.GetAsync("api/User/get-all");

			if (!response.IsSuccessStatusCode)
				return new List<UserListDTO>();

			var body = await response.Content.ReadAsStringAsync();

			if (string.IsNullOrWhiteSpace(body))
				return new List<UserListDTO>();

			return JsonSerializer.Deserialize<List<UserListDTO>>(
				body,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new List<UserListDTO>();

		}

        public async Task<UserListDTO?> GetUserByIdAsync(int userId)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/User/get-by-id/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserListDTO>(
                body,new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }

		public async Task<bool> SetActiveStatusAsync(int id, bool activeStatus)
		{
            AttachToken();
            var body = new
            {
                id = id,
                activeStatus = activeStatus
            };
            var response = await _httpClient.PostAsJsonAsync("api/User/active-status",body);
            return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateUserAsync(UserUpdateDTO userUpdateDTO)
		{
            AttachToken();
            var response = await _httpClient.PutAsJsonAsync("api/User/update",userUpdateDTO);
			return response.IsSuccessStatusCode;
		}

        public async Task<PagedResultDTO<UserListDTO>> SearchUserAsync(UserSearchDTO userSearchDTO)
        {
            AttachToken();
            var response = await _httpClient.PostAsJsonAsync("api/User/search", userSearchDTO);
            if (!response.IsSuccessStatusCode)
            {
                return new PagedResultDTO<UserListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResultDTO<UserListDTO>>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new PagedResultDTO<UserListDTO>();
        }

        public async Task<bool> SoftDeleteUserIdAsync(int id)
        {
			AttachToken();
			var response = await _httpClient.DeleteAsync($"api/User/delete/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePasswordByUserAsync(PasswordUpdateByUserDTO dto)
        {
            AttachToken();
            var response = await _httpClient
                    .PostAsJsonAsync("api/User/password-reset", dto);
            return response.IsSuccessStatusCode;

        }

        public async Task<bool> UpdatePasswordByAdminAsync(PasswordResetForAdminDTO dto)
        {
            AttachToken();
            var response = await _httpClient
                .PostAsJsonAsync("api/User/password-update", dto);

            return response.IsSuccessStatusCode;
        }

		public async Task<List<ApproverUserDTO>> GetAllApprovers()
		{
            AttachToken();
            var response = await _httpClient.GetAsync("/api/User/approvers");
            if(!response.IsSuccessStatusCode)
            {
				return new List<ApproverUserDTO>();

			}
			var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ApproverUserDTO>>(
               body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ApproverUserDTO>();
		}
	}
}
