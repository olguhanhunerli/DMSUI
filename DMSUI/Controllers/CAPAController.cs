using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.CAPA;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class CAPAController : Controller
    {
        private readonly IComplaintManager _complaintManager;

        public CAPAController(IComplaintManager complaintManager)
        {
            _complaintManager = complaintManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetComplaintSelectItems(string? search, int take = 50)
        {
            var items = await _complaintManager.GetComplaintForCapaSelect(search, take);

            items ??= new List<ComplaintForCapaSelectDTO>();

            return Json(items);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new CAPASelectListVM
            {
                Items = await _complaintManager.GetComplaintForCapaSelect(null, 50)
            };

            return View(vm);
        }
    }
}
