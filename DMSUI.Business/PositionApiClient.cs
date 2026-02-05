using DMSUI.Business.Exceptions;
using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Position;
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
	public class PositionApiClient : IPositionApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public PositionApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<PositionListDTO>> GetAllPositionsAsync()
        {
            AttachToken();
            var response = await _httpClient.GetAsync("api/Position/get-all");
            return await response.ReadAsAsync<List<PositionListDTO>>() ?? new List<PositionListDTO>();
        }

        public async Task<bool> AddPositionAsync(PositionCreateDTO positionCreateDTO)
        {
            AttachToken();
            var response = await _httpClient.PostAsJsonAsync("api/Position/create", positionCreateDTO);
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<PositionDetailDTO> GetPositionByIdAsync(int id)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Position/get-by-id/{id}");
            return await response.ReadAsAsync<PositionDetailDTO>();
        }

        public async Task<bool> DeletePositionAsync(int id)
        {
            AttachToken();
            var response = await _httpClient.DeleteAsync($"api/Position/delete/{id}");
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<bool> UpdatePositionAsync(PositionUpdateDTO positionUpdateDTO, int id)
        {
            AttachToken();
            var response = await _httpClient.PutAsJsonAsync("api/Position/update", positionUpdateDTO);
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<PagedResultDTO<PositionListDTO>> GetPagedAsync(int page, int pageSize)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Position/get-paged?page={page}&pageSize={pageSize}");
            return await response.ReadAsAsync<PagedResultDTO<PositionListDTO>>() ?? new PagedResultDTO<PositionListDTO>();
        }

    }
}
