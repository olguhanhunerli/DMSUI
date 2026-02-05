using DMSUI.Entities.DTOs.CAPA;
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
        private readonly ICAPAManager _capaManager;

        public CAPAController(IComplaintManager complaintManager, ICAPAManager capaManager)
        {
            _complaintManager = complaintManager;
            _capaManager = capaManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var entity = await _capaManager.GetAllCAPAS(page, pageSize);
            return View(entity);
        }
        [HttpGet]
        public async Task<IActionResult> GetComplaintSelectItems(string? search, int take = 50)
        {
            var items = await _complaintManager.GetComplaintForCapaSelect(search, take);

            items ??= new List<ComplaintForCapaSelectDTO>();

            return Json(items);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string complaintNo)
        {
            if (string.IsNullOrWhiteSpace(complaintNo))
                return RedirectToAction(nameof(Index));

            var form = await _capaManager.CreateFormCAPAS(complaintNo);

            if (form == null)
            {
                TempData["Error"] = "Create form verisi alınamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(form);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CAPACreateReqDTO dto)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(dto, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            }));

            try
            {
                var created = await _capaManager.CreateCAPAAsync(dto);

                TempData["Success"] = $"CAPA oluşturuldu: {created.CapaNo}";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));

            }
        }
    }
}
