using DMSUI.Services.Interfaces;
using DMSUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
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
    }
}
