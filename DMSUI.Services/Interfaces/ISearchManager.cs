using DMSUI.Entities.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface ISearchManager
    {
		Task<GlobalSearchResultDTO> SearchAsync(string query, int page, int pageSize);

	}
}
