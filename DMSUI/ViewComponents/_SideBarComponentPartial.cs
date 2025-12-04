using Microsoft.AspNetCore.Mvc;

namespace DMSUI.ViewComponents
{
    public class _SideBarComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
