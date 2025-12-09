using DMSUI.Entities.DTOs.Position;
using DMSUI.Entities.DTOs.Role;
using DMSUI.Services;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Position;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionManager _positionManager;
        private readonly ICompanyManager _companyManager;
        private readonly IUserManager _userManager;

		public PositionController(IPositionManager positionManager, ICompanyManager companyManager, IUserManager userManager)
		{
			_positionManager = positionManager;
			_companyManager = companyManager;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = await _positionManager.GetPagedAsync(page, pageSize);

            ViewBag.Page = result.Page;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalCount = result.TotalCount;

            return View(result.Items);
        }
        public async Task<IActionResult> Create()
        {
            var result = new PositionCreateViewModel
            {
                CompanyList = (await _companyManager.GetAllCompaniesAsync())
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList(),
            };
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PositionCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var dto = new PositionCreateDTO
            {
                CompanyId = model.CompanyId,
                CreatedBy = model.CreatedBy,
                Description = model.Description,
                IsActive = model.IsActive,
                Name = model.Name
            };

            await _positionManager.AddPositionAsync(dto);
            return RedirectToAction(nameof(Index)); 
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
			var position = await _positionManager.GetPositionByIdAsync(id);

			if (position == null)
				return NotFound();

			var result = new PositionEditViewModel
			{
				Id = position.Id,
				Name = position.Name,
				CompanyId = position.CompanyId,
				Description = position.Description,
                IsActive = position.IsActive,
                

				CompanyList = (await _companyManager.GetAllCompaniesAsync())
					.Select(c => new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name,
						Selected = c.Id == position.CompanyId
					}).ToList()
			};

			return View(result);
		}
        [HttpPost]
        public async Task<IActionResult> Edit(PositionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var dto = new PositionUpdateDTO
            {
               Id=vm.Id,
               Name=vm.Name,
               CompanyId=vm.CompanyId,
               Description = vm.Description,
               IsActive = vm.IsActive,
            };
            var result = await _positionManager.UpdatePositionAsync(dto, vm.Id);
            if (!result)
            {
                ModelState.AddModelError("", "Rol Güncellenemedi");
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _positionManager.DeletePositionAsync(id);

            if (!result)
                TempData["Error"] = "Pozisyon silinemedi!";
            else
                TempData["Success"] = "Rol başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}
