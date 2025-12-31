using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class AuditController : Controller
    {
        private readonly IAuditManager _auditManager;

        public AuditController(IAuditManager auditManager)
        {
            _auditManager = auditManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            var result = await _auditManager.GetDocumentAccessLogsAsync(page, pageSize);
            return View(result);
        }
    }
}
