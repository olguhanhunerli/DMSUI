using DMSUI.Entities.DTOs.Role;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Role;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleManager _roleManager;

        public RoleController(IRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.GetAllRolesAsync();
            return View(roles);
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
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _roleManager.DeleteRoleAsync(id);

			if (!result)
				return BadRequest("Silme başarısız.");

			return Ok();
		}
	}
}
