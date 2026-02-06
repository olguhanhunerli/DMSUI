using DMSUI.Entities.DTOs.CAPA;
using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Entities.DTOs.Lookups;
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
        [HttpGet]
        public async Task<IActionResult> Detail(string capaNo)
        {

            if (string.IsNullOrWhiteSpace(capaNo))
                return BadRequest("capaNo zorunlu.");

            var entity = await _capaManager.GetCAPASByCapaNo(capaNo);
            if (entity == null)
                return NotFound();

            var model = new CAPADetailDTO
            {
                Id = entity.Id,
                CapaNo = entity.CapaNo,
                ComplaintNo = entity.ComplaintNo,
                NonConformity = entity.NonConformity,
                RootCauseMethodName = entity.RootCauseMethodName,
                RootCause = entity.RootCause,
                CorrectiveAction = entity.CorrectiveAction,
                Status = entity.Status,
                OwnerId = entity.OwnerId,
                OwnerByName = entity.OwnerByName,
                CompanyId = entity.CompanyId,
                CompanyName = entity.CompanyName,
                EffectivenessCheck = entity.EffectivenessCheck,
                EffectivenessCheckedBy = entity.EffectivenessCheckedBy,
                EffectivenessCheckedByName = entity.EffectivenessCheckedByName,
                RemainingDays = entity.RemainingDays,
                DueDate = entity.DueDate,
                EffectivenessCheckedAt = entity.EffectivenessCheckedAt,
                OpenedAt = entity.OpenedAt,
                CloseAt = entity.CloseAt,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Actions = entity.Actions ?? new List<CapaActionListDTO>()
            };

            if (!string.IsNullOrWhiteSpace(model.ComplaintNo))
            {
                var form = await _capaManager.CreateFormCAPAS(model.ComplaintNo);

                if (form?.Complaint != null)
                {
                    model.ComplaintCompanyName = form.Complaint.CompanyName;
                    model.CustomerName = form.Complaint.CustomerName ?? form.Customer?.Name;
                    model.CustomerComplaintNo = form.Complaint.CustomerComplaintNo;
                    model.CustomerPO = form.Complaint.CustomerPO;
                    model.ComplaintTitle = form.Complaint.Title;

                    ViewBag.RootCauseMethods =
                    form.Lookups?.RootCauseMethods ?? new List<LookupItemDTO>();
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string capaNo)
        {

            if (string.IsNullOrWhiteSpace(capaNo))
                return BadRequest("capaNo zorunlu.");

            var entity = await _capaManager.GetCAPASByCapaNo(capaNo);
            if (entity == null)
                return NotFound();

            var model = new CAPADetailDTO
            {
                Id = entity.Id,
                CapaNo = entity.CapaNo,
                ComplaintNo = entity.ComplaintNo,
                NonConformity = entity.NonConformity,
                RootCauseMethodName = entity.RootCauseMethodName,
                RootCause = entity.RootCause,
                CorrectiveAction = entity.CorrectiveAction,
                Status = entity.Status,
                OwnerId = entity.OwnerId,
                OwnerByName = entity.OwnerByName,
                CompanyId = entity.CompanyId,
                CompanyName = entity.CompanyName,
                EffectivenessCheck = entity.EffectivenessCheck,
                EffectivenessCheckedBy = entity.EffectivenessCheckedBy,
                EffectivenessCheckedByName = entity.EffectivenessCheckedByName,
                RemainingDays = entity.RemainingDays,
                DueDate = entity.DueDate,
                EffectivenessCheckedAt = entity.EffectivenessCheckedAt,
                OpenedAt = entity.OpenedAt,
                CloseAt = entity.CloseAt,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Actions = entity.Actions ?? new List<CapaActionListDTO>()
            };

            if (!string.IsNullOrWhiteSpace(model.ComplaintNo))
            {
                var form = await _capaManager.CreateFormCAPAS(model.ComplaintNo);

                if (form?.Complaint != null)
                {
                    model.ComplaintCompanyName = form.Complaint.CompanyName;
                    model.CustomerName = form.Complaint.CustomerName ?? form.Customer?.Name;
                    model.CustomerComplaintNo = form.Complaint.CustomerComplaintNo;
                    model.CustomerPO = form.Complaint.CustomerPO;
                    model.ComplaintTitle = form.Complaint.Title;

                    ViewBag.RootCauseMethods =
                    form.Lookups?.RootCauseMethods ?? new List<LookupItemDTO>();
                }
            }

            return View(model);
        }
    }
}
