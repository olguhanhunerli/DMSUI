using DMSUI.Entities.DTOs.User;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IPositionManager _positionManager;
        private readonly IDepartmentManager _departmentManager;
        private readonly IRoleManager _roleManager;
		public UserController(IUserManager userManager, IPositionManager positionManager, IDepartmentManager departmentManager, IRoleManager roleManager)
		{
			_userManager = userManager;
			_positionManager = positionManager;
			_departmentManager = departmentManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetAllUsersAsync();
            var usersVm = users.Select(x => new UserViewModel
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,

                RoleId = x.RoleId,
                RoleName = x.RoleName,
                RoleDescription = x.RoleDescription,

                DepartmentName = x.DepartmentName,

                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,

                ManagerName = x.ManagerName,
                PositionId = x.PositionId,
                PositionName = x.PositionName,

                CanApprove = x.CanApprove,
                ApprovalLevel = x.ApprovalLevel,

                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,

                Language = x.Language,
                TimeZone = x.TimeZone,
                Theme = x.Theme,
                NotificationPreferences = x.NotificationPreferences
            }).ToList();

            return View(usersVm);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var user = await _userManager.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userVm = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                RoleName = user.RoleName,
                RoleDescription = user.RoleDescription,
                DepartmentName = user.DepartmentName,
                CompanyId = user.CompanyId,
                CompanyName = user.CompanyName,
                ManagerName = user.ManagerName,
                PositionId = user.PositionId,
                PositionName = user.PositionName,
                CanApprove = user.CanApprove,
                ApprovalLevel = user.ApprovalLevel,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted,
                Language = user.Language,
                TimeZone = user.TimeZone,
                Theme = user.Theme,
                NotificationPreferences = user.NotificationPreferences
            };
            return View(userVm);
        }
        public async Task<IActionResult> Edit(int id)
        {
			var user = await _userManager.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
			var position = await _positionManager.GetAllPositionsAsync();
            var department = await _departmentManager.GetAllDepartmentsAsync();
            var role = await _roleManager.GetAllRolesAsync();
            var manager = await _userManager.GetAllUsersAsync();
            var userVm = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                RoleName = user.RoleName,
                RoleDescription = user.RoleDescription,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.DepartmentName,
                CompanyId = user.CompanyId,
                CompanyName = user.CompanyName,
                ManagerName = user.ManagerName,
                PositionId = user.PositionId,
                PositionName = user.PositionName,
                CanApprove = user.CanApprove,
                ApprovalLevel = user.ApprovalLevel,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted,
                Language = user.Language,
                TimeZone = user.TimeZone,
                Theme = user.Theme,
                NotificationPreferences = user.NotificationPreferences,
                PositionList = position.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name,
                    Selected = p.Id == user.PositionId
                }).ToList(),
                DepartmentList = department.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name,
                    Selected = d.Id == user.DepartmentId
                }).ToList(),
                RoleList = role.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name,
                    Selected = r.Id == user.RoleId
                }).ToList(),
                ManagerList = manager.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.FullName,
                    Selected = m.Id == user.ManagerId
                }).ToList()
            };
            
			return View(userVm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel userVm)
        {
			ModelState.Remove(nameof(UserViewModel.ManagerList));
			ModelState.Remove(nameof(UserViewModel.RoleList));
			ModelState.Remove(nameof(UserViewModel.DepartmentList));
			ModelState.Remove(nameof(UserViewModel.PositionList));

			Console.WriteLine("POST Edit ÇALIŞTI"); // ✅ BUNU EKLE
			if (!ModelState.IsValid)
            {
				foreach (var modelState in ModelState)
				{
					foreach (var error in modelState.Value.Errors)
					{
						Console.WriteLine($"{modelState.Key}: {error.ErrorMessage}");
					}
				}
				userVm.PositionList = (await _positionManager.GetAllPositionsAsync())
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name,
                        Selected = p.Id == userVm.PositionId
                    }).ToList();
				userVm.DepartmentList = (await _departmentManager.GetAllDepartmentsAsync())
					.Select(d => new SelectListItem
					{
						Value = d.Id.ToString(),
						Text = d.Name,
						Selected = d.Id == userVm.DepartmentId
					}).ToList();
                userVm.RoleList = (await _roleManager.GetAllRolesAsync())
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.Name,
                        Selected = r.Id == userVm.RoleId
                    }).ToList();
                userVm.ManagerList = (await _userManager.GetAllUsersAsync())
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.FullName,
                        Selected = m.Id == userVm.ManagerId
                    }).ToList();

                return View(userVm);
			}

            var dto = new UserUpdateDTO
            {
                Id = userVm.Id,
                FirstName = userVm.FirstName,
                LastName = userVm.LastName,
                Email = userVm.Email,
                PhoneNumber = userVm.PhoneNumber,
                UserName = userVm.UserName,

                RoleId = userVm.RoleId,
                DepartmentId = userVm.DepartmentId,
                CompanyId = userVm.CompanyId,

				ManagerId = userVm.ManagerId,
				PositionId = userVm.PositionId,

                CanApprove = userVm.CanApprove,
                ApprovalLevel = userVm.ApprovalLevel,

                IsActive = userVm.IsActive,
                IsLocked = false,

                Language = userVm.Language,
                TimeZone = userVm.TimeZone,
                Theme = userVm.Theme,
                NotificationPreferences = userVm.NotificationPreferences
            };
			Console.WriteLine(JsonSerializer.Serialize(dto));
			var success = await _userManager.UpdateUserAsync(dto);
            if (!success)
            {
				ModelState.AddModelError("", "Kullanıcı Güncellenemedi");
                return View(userVm);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
