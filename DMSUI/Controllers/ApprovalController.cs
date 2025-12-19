using DMSUI.Services;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DMSUI.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly IDocumentApprovalManager _manager;
        private readonly IDocumentManager _documentManager;

		public ApprovalController(IDocumentApprovalManager manager, IDocumentManager documentManager)
		{
			_manager = manager;
			_documentManager = documentManager;
		}

		public async Task<IActionResult> Index(int page =1 , int pageSize = 10)
        {
            var list = await _manager
            .GetMyPendingApprovalAsync(page, pageSize);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.HasData = list.Any();

            return View(list);
        }
		[HttpGet]
		public async Task<IActionResult> Detail(int id)
		{
			Console.WriteLine("DETAIL ID = " + id);
			var document = await _documentManager.GetByIdAsync(id);
			if (document == null)
			{
				return NotFound();
			}
			return View(document);
		}
		[HttpPost]
		public async Task<IActionResult> Approve(int documentId)
		{
			await _manager.ApproveAsync(documentId);
			return RedirectToAction("Index");
        }
		[HttpPost]
		public async Task<IActionResult> Reject(int documentId, string reason)
		{
			await _manager.RejectAsync(documentId, reason);
			return RedirectToAction("Index");
        }
    }
}
