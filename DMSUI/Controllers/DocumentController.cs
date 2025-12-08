using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
