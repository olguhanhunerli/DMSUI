using DMSUI.Entities.DTOs.CAPA;
using DMSUI.Entities.DTOs.CapaActions;
using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Entities.DTOs.Lookups;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.CAPA;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class CAPAController : Controller
    {
        private readonly IComplaintManager _complaintManager;
        private readonly ICAPAManager _capaManager;
        private readonly ICapaActionsManager _capaActionsManager;

        public CAPAController(IComplaintManager complaintManager, ICAPAManager capaManager, ICapaActionsManager capaActionsManager)
        {
            _complaintManager = complaintManager;
            _capaManager = capaManager;
            _capaActionsManager = capaActionsManager;
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
        [HttpPost]
        public async Task<IActionResult> Edit(CAPADetailDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CapaNo))
                return BadRequest("capaNo zorunlu.");


            var model = new CAPAUpdateReqDTO
            {
                NonConformity = dto.NonConformity,
                RootCauseMethodId = dto.RootCauseMethodId,
                RootCause = dto.RootCause,
                CorrectiveAction = dto.CorrectiveAction,
                DueDate = dto.DueDate,
                OwnerId = dto.OwnerId,
                Status = "BEKLIYOR",
                EffectivenessCheck = dto.EffectivenessCheck,
                EffectivenessCheckedBy = dto.EffectivenessCheckedBy,
                EffectivenessCheckedAt = dto.EffectivenessCheckedAt,
                EffectivenessResult = dto.EffectivenessResult
            };

            try
            {
                var ok = await _capaManager.UpdateCAPAAsync(dto.CapaNo, model);

                if (ok)
                {
                    TempData["Success"] = "CAPA güncellendi.";
                    return RedirectToAction(nameof(Detail), new { capaNo = dto.CapaNo });
                }

                TempData["Error"] = "Güncelleme başarısız (API).";
                return RedirectToAction(nameof(Edit), new { capaNo = dto.CapaNo });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Edit), new { capaNo = dto.CapaNo });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAction(string capaNo, CreateCapaActionDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(capaNo))
                    return BadRequest(new { ok = false, message = "CAPA No boş olamaz." });

                if (string.IsNullOrWhiteSpace(dto.ActionType) || string.IsNullOrWhiteSpace(dto.Description))
                    return BadRequest(new { ok = false, message = "Aksiyon tipi ve açıklama zorunludur." });

                var created = await _capaActionsManager.CreateCapaActionAsync(capaNo, dto);

                if (created == null)
                    return StatusCode(500, new { ok = false, message = "Aksiyon ekleme başarısız (API)." });

                // UI tabloya ekleyebilsin diye created'ı geri dön
                return Ok(new { ok = true, message = "Aksiyon eklendi.", data = created });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ok = false, message = ex.Message });
            }
        }

    }
}
