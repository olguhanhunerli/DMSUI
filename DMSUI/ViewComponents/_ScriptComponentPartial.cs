using Microsoft.AspNetCore.Mvc;

namespace DMSUI.ViewComponents
{
    public class _ScriptComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
