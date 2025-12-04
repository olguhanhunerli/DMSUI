using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
