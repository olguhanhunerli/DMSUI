using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchManager _manager;

		public SearchController(ISearchManager manager)
		{
			_manager = manager;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string query, int page = 1, int pageSize = 10)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return View(new Entities.DTOs.Search.GlobalSearchResultDTO());
			}
			var result = await _manager.SearchAsync(query, page, pageSize);
			return View(result);
		}

	}
}
