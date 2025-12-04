using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Role;
using DMSUI.Entities.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
	public class RoleApiClient : IRoleApiClient
	{
		private readonly HttpClient _httpClient;

		public RoleApiClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<RoleListDTO>> GetRoleListsAsync()
		{
			var response = await _httpClient.GetAsync("api/Role/GetAllRoles");
			if (!response.IsSuccessStatusCode)
			{
				return new List<RoleListDTO>();
			}
			var body = await response.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<List<RoleListDTO>>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new List<RoleListDTO>();
		}
	}
}
