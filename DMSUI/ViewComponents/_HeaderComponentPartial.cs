using Microsoft.AspNetCore.Mvc;

namespace DMSUI.ViewComponents
{
    public class _HeaderComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
