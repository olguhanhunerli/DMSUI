using DMSUI.Services;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentManager _documentManager;

        public DocumentController(IDocumentManager manager)
        {
            _documentManager = manager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {

            var result = await _documentManager.GetPagedAsync(page, pageSize);

            ViewBag.Page = result.Page;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalCount = result.TotalCount;

            return View(result.Items);
        }
    }
}
