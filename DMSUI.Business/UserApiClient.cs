using DMSUI.Business.Exceptions;
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
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<List<UserListDTO>> GetAllUsersAsync()
        {
            AttachToken();
            var response = await _httpClient.GetAsync("api/User/get-all");
            return await response.ReadAsAsync<List<UserListDTO>>() ?? new List<UserListDTO>();
        }

        public async Task<UserListDTO?> GetUserByIdAsync(int userId)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/User/get-by-id/{userId}");
            return await response.ReadAsAsync<UserListDTO>();
        }

        public async Task<bool> SetActiveStatusAsync(int id, bool activeStatus)
        {
            AttachToken();
            var body = new { id, activeStatus };
            var response = await _httpClient.PostAsJsonAsync("api/User/active-status", body);
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDTO userUpdateDTO)
        {
            AttachToken();
            var response = await _httpClient.PutAsJsonAsync("api/User/update", userUpdateDTO);
            return await response.EnsureSuccessOrThrowAsync();
        }
        public async Task<PagedResultDTO<UserListDTO>> SearchUserAsync(UserSearchDTO userSearchDTO)
        {
            AttachToken();
            var response = await _httpClient.PostAsJsonAsync("api/User/search", userSearchDTO);
            return await response.ReadAsAsync<PagedResultDTO<UserListDTO>>() ?? new PagedResultDTO<UserListDTO>();
        }

        public async Task<bool> SoftDeleteUserIdAsync(int id)
        {
            AttachToken();
            var response = await _httpClient.DeleteAsync($"api/User/delete/{id}");
            return await response.EnsureSuccessOrThrowAsync();
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
            return await response.ReadAsAsync<List<ApproverUserDTO>>() ?? new List<ApproverUserDTO>();
        }
    }
}
