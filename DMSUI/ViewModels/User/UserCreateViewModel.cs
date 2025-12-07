using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.User
{
    public class UserCreateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }
        public int? ManagerId { get; set; }
        public int PositionId { get; set; }

        public bool CanApprove { get; set; }
        public int ApprovalLevel { get; set; }

        public string Language { get; set; } = "tr";
        public string TimeZone { get; set; } = "Europe/Istanbul";
        public string Theme { get; set; } = "light";
        public string NotificationPreferences { get; set; } = "email,push";

        public List<SelectListItem> RoleList { get; set; } = new();
        public List<SelectListItem> DepartmentList { get; set; } = new();
        public List<SelectListItem> PositionList { get; set; } = new();
        public List<SelectListItem> ManagerList { get; set; } = new();
        public List<SelectListItem> CompanyList { get; set; } = new();
    }
}
