using DMSUI.Entities.DTOs.Document;
using DMSUI.Services;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Document;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentManager _documentManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IDepartmentManager _departmentManager;
        private readonly IUserManager _userManager;
		public DocumentController(IDocumentManager manager, ICategoryManager categoryManager, IUserManager userManager, IDepartmentManager departmentManager)
		{
			_documentManager = manager;
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

			var userName = jwt.Claims.First(x => x.Type == "userName").Value;
			var user = await _userManager.GetUserByIdAsync(userId);
			DocumentCreatePreviewDTO previewDTO = await _documentManager.GetDocumentCreatePreview(categoryId);
            if (previewDTO == null)
            {
                return NotFound();
            }

            var vm = new DocumentCreatePreviewViewModel
            {
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

				DepartmentList = (await _departmentManager.GetAllDepartmentsAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList(),
            };
            return View(vm);
        }
    }
}
