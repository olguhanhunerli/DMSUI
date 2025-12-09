using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.ViewComponents
{
    public class _CategoryComponentPartial: ViewComponent
    {
        public readonly ICategoryManager _categoryManager;

        public _CategoryComponentPartial(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryManager.GetTreeAsync();
            return View(categories);
        }
    }
}
