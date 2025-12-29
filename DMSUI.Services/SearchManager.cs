using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Search;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
	public class SearchManager : ISearchManager
	{
		private readonly ISearchApiClient _client;

		public SearchManager(ISearchApiClient client)
		{
			_client = client;
		}

		public async Task<GlobalSearchResultDTO> SearchAsync(string query, int page, int pageSize)
		{
			return await _client.SearchAsync(query, page, pageSize);
		}
	}
}
