using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class DashboardController : Controller
    {
		private readonly IDocumentApprovalManager _manager;
		private readonly IDocumentManager _documentManager;

		public DashboardController(IDocumentApprovalManager manager, IDocumentManager documentManager)
		{
			_manager = manager;
			_documentManager = documentManager;
		}

		public async Task<IActionResult> Index(string tab = "approval")
		{
			ViewBag.ActiveTab = tab;

			var approvals = await _manager.GetMyPendingApprovalAsync(1, 5);
			ViewBag.PendingApprovals = approvals;

			ViewBag.PendingApprovalCount = approvals.Count; 

			ViewBag.PendingReadCount = 0;
			ViewBag.PendingReads = new List<object>();

			var myRevisions = await _documentManager.MyActiveRevisionAsync(1, 5);
			ViewBag.PendingRevisions = myRevisions.Items;
			ViewBag.PendingRevisionsCount = myRevisions.TotalCount;

			return View();
		}
	}
}
