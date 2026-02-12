using DMSUI.Entities.DTOs.CAPA;
using DMSUI.Entities.DTOs.CapaActions;
using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Entities.DTOs.Lookups;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.CAPA;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class CAPAController : Controller
    {
        private readonly IComplaintManager _complaintManager;
        private readonly ICAPAManager _capaManager;
        private readonly ICapaActionsManager _capaActionsManager;
        private readonly ICapaActionFilesManager _capaActionFilesManager;
        private readonly IUserManager _userManager;
        private readonly ICapaEvidenceFilesManager _evidenceFilesManager;
        public CAPAController(IComplaintManager complaintManager, ICAPAManager capaManager, ICapaActionsManager capaActionsManager, ICapaActionFilesManager capaActionFilesManager, IUserManager userManager, ICapaEvidenceFilesManager evidenceFilesManager)
        {
            _complaintManager = complaintManager;
            _capaManager = capaManager;
            _capaActionsManager = capaActionsManager;
            _capaActionFilesManager = capaActionFilesManager;
            _userManager = userManager;
            _evidenceFilesManager = evidenceFilesManager;
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
                Actions = entity.Actions ?? new List<CapaActionListDTO>(),
                Files = entity.Files ?? new List<CapaEvidenceFilesListDTO>()
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
                Actions = entity.Actions ?? new List<CapaActionListDTO>(),
                Files = entity.Files ?? new List<CapaEvidenceFilesListDTO>(),
                
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
            ViewBag.Users = (await _userManager.GetAllUsersAsync())
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName 
                })
                .ToList();
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

                return Ok(new { ok = true, message = "Aksiyon eklendi.", data = created });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ok = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ComplateAction(int actionId, string status)
        {
            var result = await _capaActionsManager.ComplateActionAsync(actionId, status);

            return Ok(new { ok = result });
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(int actionId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya Seçilmedi");
            var ok = await _capaActionFilesManager.CreateFile(actionId, file);
            return Ok(new { ok });
        }
        [HttpPost]
        public async Task<IActionResult> CloseCapa(string capaNo, CloseCapaDTO dto)
        {
            if (string.IsNullOrWhiteSpace(capaNo))
                return BadRequest();

            var actions = await _capaActionsManager.GetCapaActionAsync(capaNo);
            if (actions.Count == 0)
            {
                TempData["Error"] = "En az 1 aksiyon girilmeden DÖF kapatılamaz.";
                return RedirectToAction(nameof(Edit), new { capaNo });
            }

            if (actions.Any(a => !string.Equals(a.Status, "TAMAMLANDI", StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = "DÖF kapatılamaz: Aksiyonlar devam ediyor.";
                return RedirectToAction(nameof(Edit), new { capaNo });
            }

            if (dto is null)
            {
                TempData["Error"] = "Geçersiz form verisi.";
                return RedirectToAction(nameof(Edit), new { capaNo });
            }

            if (!ModelState.IsValid)
            {
                var errs = string.Join(" | ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = "Form validation: " + errs;
                return RedirectToAction(nameof(Edit), new { capaNo });
            }

            try
            {
                await _capaManager.ClosedCapaAsync(capaNo, dto);
                TempData["Success"] = "DÖF başarı ile kapatıldı";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Edit), new { capaNo });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadDofFile(string capaNo, IFormFile File) 
        {
            if (string.IsNullOrWhiteSpace(capaNo))
                return BadRequest("capaNo boş olamaz.");

            if (File == null || File.Length == 0)
                return BadRequest("Dosya seçmelisin.");

            await _evidenceFilesManager.CreateEvidenceFiles(capaNo, File);
            return Ok();
        }
        
    }
}
