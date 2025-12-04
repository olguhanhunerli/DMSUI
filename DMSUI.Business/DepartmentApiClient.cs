using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
	public class DepartmentApiClient : IDepartmentApiClient
	{
		private readonly HttpClient _httpClient;

		public DepartmentApiClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<DepartmentListDTO>> GetAllDepartmentsAsync()
		{
			var response = await _httpClient.GetAsync("api/Department/get-all-departments");
			if (!response.IsSuccessStatusCode)
			{
				return new List<DepartmentListDTO>();
			}
			var body = await response.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<List<DepartmentListDTO>>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new List<DepartmentListDTO>();
		}
	}
}
