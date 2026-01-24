using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
	public class InstrumentController: Controller
	{
		private readonly IInstrumentManager _manager;
		public InstrumentController(IInstrumentManager manager)
		{
			_manager = manager;
		}
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var list = await _manager.GetInstrumentsAsync(page, pageSize);
			ViewBag.Page = page;
			ViewBag.PageSize = pageSize;
			ViewBag.HasData = list.Items.Any();
			return View(list);
		}
	}
}
