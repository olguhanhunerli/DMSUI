using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DMSUI.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly IDocumentApprovalManager _manager;

        public ApprovalController(IDocumentApprovalManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> IndexAsync(int page =1 , int pageSize = 10)
        {
            var list = await _manager
            .GetMyPendingApprovalAsync(page, pageSize); // SENİN LIST METODUN

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.HasData = list.Any();

            return View(list);
        }
    }
}
