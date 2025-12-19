using DMSUI.Entities.DTOs.Document;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Document;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
   
    public class DocumentController : Controller
    {
        private readonly IDocumentManager _documentManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IDepartmentManager _departmentManager;
        private readonly IUserManager _userManager;

        public DocumentController(
            IDocumentManager documentManager,
            ICategoryManager categoryManager,
            IUserManager userManager,
            IDepartmentManager departmentManager)
        {
            _documentManager = documentManager;
            _categoryManager = categoryManager;
            _userManager = userManager;
            _departmentManager = departmentManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = await _documentManager.GetPagedAsync(page, pageSize);

            ViewBag.Page = result.Page;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalCount = result.TotalCount;

            return View(result.Items);
        }

        [HttpGet]
        public async Task<IActionResult> SelectCategory()
        {
            var categories = await _categoryManager.GetTreeAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int categoryId)
        {
            if (categoryId <= 0)
            {
                return RedirectToAction("SelectCategory");
            }

            var token = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token Bilgisi Alınamadı");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userId = int.Parse(
                jwt.Claims.First(x => x.Type == "userId" || x.Type == "sub").Value
            );

            var user = await _userManager.GetUserByIdAsync(userId);

            var previewDTO = await _documentManager.GetDocumentCreatePreview(categoryId);
            if (previewDTO == null)
            {
                return NotFound();
            }

            var approvers = await _userManager.GetAllApprovers();
            var departments = await _departmentManager.GetAllDepartmentsAsync();

            var vm = new DocumentCreatePreviewViewModel
            {
                CategoryId = categoryId, 
                DocumentCode = previewDTO.DocumentCode,
                CompanyName = previewDTO.CompanyName,
                CategoryName = previewDTO.CategoryName,
                CategoryBreadcrumb = previewDTO.CategoryBreadcrumb,
                VersionNumber = previewDTO.VersionNumber,
                IsCodeValid = previewDTO.IsCodeValid,

                PreparedByUserId = user.Id,
                PreparedByUserName = user.UserName,
                PreparedAt = DateTime.Now,

                RevisionNumber = 0,

                DepartmentList = departments
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                    .ToList()
            };

            vm.ApprovalList = approvers.Select(x => new ApprovalRowViewModel
            {
                UserId = x.Id,
                UserName = x.FullName,
                PositionName = x.PositionName,
                ApprovalLevel = 1
            }).ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
             DocumentCreatePreviewViewModel vm,
             IFormFile? DocumentFile,
             List<IFormFile>? AttachmentFiles)
        {
            var preview = await _documentManager.GetDocumentCreatePreview(vm.CategoryId);

            vm.DocumentCode = preview.DocumentCode;
            vm.CompanyName = preview.CompanyName;
            vm.CategoryName = preview.CategoryName;
            vm.CategoryBreadcrumb = preview.CategoryBreadcrumb;
            vm.VersionNumber = preview.VersionNumber;
            vm.IsCodeValid = preview.IsCodeValid;

            if (string.IsNullOrWhiteSpace(vm.DocumentCode))
            {
                ModelState.AddModelError("", "Doküman kodu alınamadı.");
                await PrepareCreateViewModelAsync(vm);
                return View(vm);
            }

            if (DocumentFile != null &&
                !DocumentFile.FileName.StartsWith(vm.DocumentCode, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("DocumentFile",
                    $"Ana dosya adı doküman kodu ({vm.DocumentCode}) ile başlamalıdır.");

                await PrepareCreateViewModelAsync(vm);
                return View(vm);
            }

            if (vm.DepartmentId == null || vm.DepartmentId <= 0)
            {
                ModelState.AddModelError("DepartmentId", "Departman seçilmelidir.");
                await PrepareCreateViewModelAsync(vm);
                return View(vm);
            }

            var selectedApproverIds = vm.ApprovalList
                .Where(a => a.IsSelected)
                .OrderBy(a => a.ApprovalLevel)
                .Select(a => a.UserId)
                .ToList();

            if (!selectedApproverIds.Any())
            {
                ModelState.AddModelError("", "En az bir onaylayıcı seçmelisiniz.");
                await PrepareCreateViewModelAsync(vm);
                return View(vm);
            }

			var createDto = new CreateDocumentDTO
			{
				TitleTr = vm.TitleTr,
				TitleEn = vm.TitleEn,
				CategoryId = vm.CategoryId,
				DepartmentId = vm.DepartmentId.Value,
				VersionNote = vm.VersionNote,
				RevisionNumber = vm.RevisionNumber,
				IsPublic = false,
				ApproverUserIds = selectedApproverIds,
				MainFile = DocumentFile,                
				Attachments = AttachmentFiles           
			};

            try
            {
                var created = await _documentManager.CreateAsync(createDto);
				Console.WriteLine("DOCUMENT CREATE RESULT --- UI: " + created);
			}
            catch (Exception ex)
            {
                ModelState.AddModelError("", "API Hatası: " + ex.Message);
                await PrepareCreateViewModelAsync(vm);
                return View(vm);
            }

            TempData["Success"] = "Doküman başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Pdf(int id)
        {
			var pdfResult = await _documentManager.GetPdfAsync(id);
			if (pdfResult == null)
			{
				return NotFound();
			}
            return File(
                pdfResult.FileBytes,
                "application/pdf");
		}


        private async Task PrepareCreateViewModelAsync(DocumentCreatePreviewViewModel vm)
        {
            var preview = await _documentManager.GetDocumentCreatePreview(vm.CategoryId);

            vm.DocumentCode = preview.DocumentCode;
            vm.CompanyName = preview.CompanyName;
            vm.CategoryName = preview.CategoryName;
            vm.CategoryBreadcrumb = preview.CategoryBreadcrumb;
            vm.VersionNumber = preview.VersionNumber;
            vm.IsCodeValid = preview.IsCodeValid;

            var departments = await _departmentManager.GetAllDepartmentsAsync();
            vm.DepartmentList = departments
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                .ToList();

            var approvers = await _userManager.GetAllApprovers();

            var selectedMap = vm.ApprovalList?.ToDictionary(a => a.UserId)
                              ?? new Dictionary<int, ApprovalRowViewModel>();

            vm.ApprovalList = approvers.Select(x =>
            {
                if (selectedMap.TryGetValue(x.Id, out var existing))
                {
                    return existing;
                }

                return new ApprovalRowViewModel
                {
                    UserId = x.Id,
                    UserName = x.FullName,
                    PositionName = x.PositionName,
                    ApprovalLevel = 1,
                    IsSelected = false
                };
            }).ToList();
        }
       
	}
}
