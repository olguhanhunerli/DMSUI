using DMSUI.Entities.DTOs.Role;
using DMSUI.Services;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    [Authorize(Roles = "Admin,SUPER_ADMIN")]
    public class RoleController : Controller
    {
        private readonly IRoleManager _roleManager;

        public RoleController(IRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = await _roleManager.GetPagedAsync(page, pageSize);

            ViewBag.Page = result.Page;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalCount = result.TotalCount;

            return View(result.Items);
        }
        public async Task<IActionResult> Create()
        {
            return View(new RoleViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var dto = new RoleCreateDTO
            { Description = model.Description , Name = model.Name};
            var result = await _roleManager.CreateRoleAsync(dto);
            if(!result)
            {
                ModelState.AddModelError("", "Rol Eklenemedi");
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var roles = await _roleManager.GetRoleByIdAsync(id);
            if (roles == null)
            {
                return NotFound();
            }
			var model = new RoleViewModel
			{
				Id = roles.Id,
				Name = roles.Name,
				Description = roles.Description
			};

			return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var dto = new RoleUpdateDTO
            {
                Name = model.Name,
                Description = model.Description
            };
            var result = await _roleManager.UpdateRoleAsync(model.Id, dto);
            if (!result)
            {
                ModelState.AddModelError("", "Rol Güncellenemedi");
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleManager.DeleteRoleAsync(id);

            if (!result)
                TempData["Error"] = "Rol silinemedi!";
            else
                TempData["Success"] = "Rol başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}
