using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
	public class PositionApiClient : IPositionApiClient
	{
		private readonly HttpClient _httpClient;

		public PositionApiClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<PositionListDTO>> GetAllPositionsAsync()
		{
			var response = await _httpClient.GetAsync("api/Position/GetAllPosition");
			if(!response.IsSuccessStatusCode)
			{
				return new List<PositionListDTO>();
			}
			var body = await response.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<List<PositionListDTO>>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new List<PositionListDTO>();

		}
	}
}
