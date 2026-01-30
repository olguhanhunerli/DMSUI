using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly IComplaintManager _complaintManager;
        private readonly ICustomerManager _customerManager;

        public ComplaintController(IComplaintManager complaintManager, ICustomerManager customerManager)
        {
            _complaintManager = complaintManager;
            _customerManager = customerManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var entity = await _complaintManager.GetComplaintsPaging(page, pageSize);
            return View(entity);
        }
        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    var vm = new CreateComplaintVM();
        //    var companyList = await _customerManager.GetAllCustomersMini(GetCompanyIdSafe);
        //    FillStaticLookups(vm);
        //    return View(vm);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateComplaintVM vm)
        {
            if (!ModelState.IsValid)
            {
                FillStaticLookups(vm);
                return View(vm);
            }

            var dto = new CreateComplaintDTO
            {
                companyId = vm.CompanyId,
                customerId = vm.CustomerId,
                channelId = vm.ChannelId,
                typeId = vm.TypeId,
                severityId = vm.SeverityId,
                title = vm.Title,
                description = vm.Description,
                isRepeat = vm.IsRepeat,
                interimActionRequired = vm.InterimActionRequired,
                interimActionNote = vm.InterimActionNote,
                assignedTo = vm.AssignedTo
            };

            var ok = await _complaintManager.CreateComplaint(dto);

            if (ok) return RedirectToAction("Index");

            ModelState.AddModelError("", "Create complaint failed");
            FillStaticLookups(vm);
            return View(vm);
        }
        private void FillStaticLookups(CreateComplaintVM vm)
        {

            vm.Channels = new List<SelectListItem>
            {
                new("Telefon", "1"),
                new("Email", "2"),
                new("Web", "3"),
                new("Whatsapp", "4"),
            };

            vm.Types = new List<SelectListItem>
            {
                new("Ürün", "1"),
                new("Hizmet", "2"),
                new("Teslimat", "3"),
                new("Diğer", "4"),
            };

            vm.Severities = new List<SelectListItem>
            {
                new("Düşük", "1"),
                new("Orta", "2"),
                new("Yüksek", "3"),
                new("Kritik", "4"),
            };
        }
    }
}
