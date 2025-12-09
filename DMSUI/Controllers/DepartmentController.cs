using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Services;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Department;
using DMSUI.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentManager _departmentManager;
		private readonly IUserManager _userManager;
		private readonly ICompanyManager _companyManager;
		public DepartmentController(IDepartmentManager departmentManager, IUserManager userManager, ICompanyManager companyManager)
		{
			_departmentManager = departmentManager;
			_userManager = userManager;
			_companyManager = companyManager;
		}

		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {

            var result = await _departmentManager.GetPagedAsync(page, pageSize);

            ViewBag.Page = result.Page;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalCount = result.TotalCount;

            return View(result.Items);
        }
		public async Task<IActionResult> Create()
		{
			var vm = new DepartmentCreateViewModel
			{
				CompanySelectList = (await _companyManager.GetAllCompaniesAsync())
				.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList(),
				ManagerSelectList = (await _userManager.GetAllUsersAsync())
				.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.FullName }).ToList(),
			};
			return View(vm);
		}
		[HttpPost]
		public async Task<IActionResult> Create(DepartmentCreateViewModel model)
		{
			
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var dto = new DepartmentCreateDTO
			{
				CompanyId = model.CompanyId,
				DepartmentCode = model.DepartmentCode,
				Description = model.Description,
				ManagerId = model.ManagerId,
				Name = model.Name,
				Notes = model.Notes,
				SortOrder = model.SortOrder,
			};
			await _departmentManager.CreateDepartmentAsync(dto);

			return RedirectToAction(nameof(Index));
		}
        public async Task<IActionResult> Edit(int id)
        {
			var department = await _departmentManager.GetDepartmentsByIdAsync(id);
			if (department == null)
				return NotFound();

			var vm = new DepartmentEditViewModel
			{
				Id = department.Id,
				Name = department.Name,
				DepartmentCode = department.DepartmentCode,
				ManagerId = department.ManagerId,
				CompanyId = (int)department.CompanyId,
				CompanyName = department.CompanyName,
				Description = department.Description,
				Notes = department.Notes,
				SortOrder = department.SortOrder,

				Users = department.Users?.Select(u => new UserMiniViewModel
				{
					Id = u.Id,
					FullName = u.FullName,
					Email = u.Email
				}).ToList() ?? new List<UserMiniViewModel>()
			};

			var managers = await _userManager.GetAllUsersAsync();

			vm.ManagerSelectList = managers.Select(u => new SelectListItem
			{
				Text = u.FullName,
				Value = u.Id.ToString(),
				Selected = (u.Id == vm.ManagerId)
			}).ToList();
			return View(vm);
        }
		[HttpPost]
		public async Task<IActionResult> Edit(DepartmentEditViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				
				var managers = await _userManager.GetAllUsersAsync();
				vm.ManagerSelectList = managers.Select(u => new SelectListItem
				{
					Text = u.FullName,
					Value = u.Id.ToString(),

				}).ToList();
				return View("Edit", vm);
			}

			var updateVm = new DepartmentUpdateDTO
			{
				Id = vm.Id,
				Name = vm.Name,
				DepartmentCode = vm.DepartmentCode,
				CompanyId = vm.CompanyId,
				ManagerId = vm.ManagerId,
				Description = vm.Description,
				Notes = vm.Notes,
				SortOrder = vm.SortOrder,
				IsActive = true
			};
			var updateDto = new DepartmentUpdateDTO
			{
				Id = updateVm.Id,
				Name = updateVm.Name,
				DepartmentCode = updateVm.DepartmentCode,
				CompanyId = updateVm.CompanyId,
				ManagerId = updateVm.ManagerId,
				Description = updateVm.Description,
				Notes = updateVm.Notes,
				SortOrder = updateVm.SortOrder,
				IsActive = updateVm.IsActive
			};
			var result = await _departmentManager.UpdateDepartmentAsync(updateDto);

			if (!result)
			{
				TempData["Error"] = "Departman güncelleme başarısız.";
				return RedirectToAction(nameof(Edit), new { id = vm.Id });
			}

			TempData["Success"] = "Departman başarıyla güncellendi.";
			return RedirectToAction(nameof(Index));
		}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentManager.DeleteDepartmentAsync(id);

            if (!result)
                TempData["Error"] = "Silme işlemi başarısız!";
            else
                TempData["Success"] = "Departman silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}
