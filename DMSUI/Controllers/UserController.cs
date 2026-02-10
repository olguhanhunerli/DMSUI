using DMSUI.Entities.DTOs.User;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly ICompanyManager _companyManager;
        public UserController(IUserManager userManager, IPositionManager positionManager, IDepartmentManager departmentManager, IRoleManager roleManager, ICompanyManager companyManager)
        {
            _userManager = userManager;
            _positionManager = positionManager;
            _departmentManager = departmentManager;
            _roleManager = roleManager;
            _companyManager = companyManager;
        }

        public async Task<IActionResult> Index(UserSearchDTO search)
        {
            ViewBag.SearchRequest = search;
            ViewBag.Roles = await _roleManager.GetAllRolesAsync();
            ViewBag.Departments = await _departmentManager.GetAllDepartmentsAsync();
            ViewBag.Companies = await _companyManager.GetAllCompaniesAsync();

            var result = await _userManager.SearchUserAsync(search);

            ViewBag.TotalPages = result.TotalPages;
            ViewBag.CurrentPage = result.Page;

            var usersVm = result.Items
                .OrderBy(x => x.Id)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    PhoneNumber = x.PhoneNumber,
                    RoleName = x.RoleName,
                    DepartmentName = x.DepartmentName,
                    CompanyName = x.CompanyName,
                    ManagerName = x.ManagerName,
                    PositionName = x.PositionName,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted == false || x.IsDeleted == null,
                    CanApprove = x.CanApprove,
                    ApprovalLevel = x.ApprovalLevel
                })
                .ToList();
            return View(usersVm);
        }
        [HttpPost]
        public async Task<IActionResult> SetActiveStatus([FromBody] UserSetActiveStatusDTO request)
        {
            var result = await _userManager.SetActiveStatusAsync(
                request.Id,
                request.IsActive
            );

            if (!result)
                return BadRequest();

            return Ok();
        }
        public async Task<IActionResult> CreateUser(int id)
        {
            var vm = new UserCreateViewModel
            {
                RoleList = (await _roleManager.GetAllRolesAsync())
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name }).ToList(),
                DepartmentList = (await _departmentManager.GetAllDepartmentsAsync())
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList(),
                ManagerList = (await _userManager.GetAllUsersAsync())
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.FullName }).ToList(),
                PositionList = (await _positionManager.GetAllPositionsAsync())
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                CompanyList = (await _companyManager.GetAllCompaniesAsync())
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
            };
            return View(vm);
          
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateViewModel vm)
        {
         
            ModelState.Remove(nameof(UserCreateViewModel.RoleList));
            ModelState.Remove(nameof(UserCreateViewModel.DepartmentList));
            ModelState.Remove(nameof(UserCreateViewModel.PositionList));
            ModelState.Remove(nameof(UserCreateViewModel.ManagerList));
            ModelState.Remove(nameof(UserCreateViewModel.CompanyList));
            if (!ModelState.IsValid)
            {
                vm.RoleList = (await _roleManager.GetAllRolesAsync())
                    .Select (p => new SelectListItem {Value = p.Id.ToString(), Text=p.Name}).ToList();
                vm.DepartmentList = (await _departmentManager.GetAllDepartmentsAsync())
                    .Select (p => new SelectListItem {Value=p.Id.ToString(), Text=p.Name}).ToList();
                vm.PositionList = (await _positionManager.GetAllPositionsAsync())
                    .Select(p => new SelectListItem { Value=p.Id.ToString(),Text=p.Name}).ToList();
                vm.CompanyList =(await _companyManager.GetAllCompaniesAsync())
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name}).ToList();
                vm.ManagerList = (await _userManager.GetAllUsersAsync())
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.FullName}).ToList();
                return View(vm);
            }
            var dto = new UserRegisterDTO
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                UserName = vm.UserName,
                Password = vm.Password,

                RoleId = vm.RoleId,
                DepartmentId = vm.DepartmentId,
                CompanyId = vm.CompanyId,
                ManagerId = vm.ManagerId,
                PositionId = vm.PositionId,

                CanApprove = vm.CanApprove,
                ApprovalLevel = vm.ApprovalLevel,

                Language = vm.Language,
                TimeZone = vm.TimeZone,
                Theme = vm.Theme,
                NotificationPreferences = vm.NotificationPreferences
            };
            var success = await _userManager.CreateUserAsync(dto);
            if (!success)
            {
                vm.RoleList = (await _roleManager.GetAllRolesAsync())
                   .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
                vm.DepartmentList = (await _departmentManager.GetAllDepartmentsAsync())
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
                vm.PositionList = (await _positionManager.GetAllPositionsAsync())
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
                vm.CompanyList = (await _companyManager.GetAllCompaniesAsync())
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                vm.ManagerList = (await _userManager.GetAllUsersAsync())
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.FullName }).ToList();
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
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
                IsDeleted = user.IsDeleted ?? false,
                Language = user.Language,
                TimeZone = user.TimeZone,
                Theme = user.Theme,
                NotificationPreferences = user.NotificationPreferences,
                PositionList = (await _positionManager.GetAllPositionsAsync())
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                DepartmentList = (await _departmentManager.GetAllDepartmentsAsync())
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList(),
                ManagerList = (await _userManager.GetAllUsersAsync())
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.FullName }).ToList(),
                RoleList = (await _roleManager.GetAllRolesAsync())
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name}).ToList()

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
                ManagerId = (int)user.ManagerId,
                ManagerName = user.ManagerName,
                PositionId = user.PositionId,
                PositionName = user.PositionName,
                CanApprove = user.CanApprove,
                ApprovalLevel = user.ApprovalLevel,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted ?? false,
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
        [HttpDelete]
        public async Task<IActionResult>DeleteUser(int id)
        {
            var result = await _userManager.SoftDeleteUserIdAsync(id);
            if (!result)
                return BadRequest("Silme İşlemi Başarısız.");
            return Ok();
        }
        public async Task<IActionResult> ChangePassword()
        {
            var token = Request.Cookies["access_token"];

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var email = jwt.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;

            var model = new PasswordUpdateByUserDTO
            {
                Email = email
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(PasswordUpdateByUserDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var success = await _userManager.UpdatePasswordByUserAsync(dto);

            if (!success)
            {
                ViewBag.Error = "Mevcut şifre hatalı!";
                return View(dto);
            }

            ViewBag.Success = "Şifre başarıyla güncellendi.";
            return View();
        }
        public IActionResult AdminResetPassword(string email)
        {
            var model = new PasswordResetForAdminDTO
            {
                Email = email
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminResetPassword(PasswordResetForAdminDTO dto)
        {
            var success = await _userManager.UpdatePasswordByAdminAsync(dto);

            if (!success)
            {
                ViewBag.Error = "Şifre sıfırlanamadı!";
                return View(dto);
            }

            ViewBag.Success = "Şifre başarıyla sıfırlandı.";
            return View(dto);
        }

    }
}
