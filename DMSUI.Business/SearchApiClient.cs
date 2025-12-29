using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Search;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
	public class SearchApiClient : ISearchApiClient
	{
		private readonly HttpClient _client;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SearchApiClient(HttpClient client, IHttpContextAccessor httpContextAccessor)
		{
			_client = client;
			_httpContextAccessor = httpContextAccessor;
		}

		private void AttachToken()
		{
			var token = _httpContextAccessor.HttpContext?
			.Request.Cookies["access_token"];

			_client.DefaultRequestHeaders.Remove("Authorization");

			if (!string.IsNullOrWhiteSpace(token))
			{
				_client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);
			}
		}
		public async Task<GlobalSearchResultDTO> SearchAsync(string query, int page, int pageSize)
		{
			AttachToken();
			var response = await _client.GetAsync($"api/Search/global?query={Uri.EscapeDataString(query)}&page={page}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				return new GlobalSearchResultDTO();
			}
			var body = await response.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<GlobalSearchResultDTO>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new GlobalSearchResultDTO();
		}
	}
}
