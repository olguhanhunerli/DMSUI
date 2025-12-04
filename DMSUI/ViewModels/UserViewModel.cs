using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
		[ValidateNever]
		public List<SelectListItem> RoleList { get; set; } = new List<SelectListItem>();
		public int DepartmentId { get; set; }
		public string? DepartmentName { get; set; }
		[ValidateNever]
		public List<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();
        public int CompanyId { get; set; } 
        public string? CompanyName { get; set; }
        public int ManagerId { get; set; } 
		public string? ManagerName { get; set; }
		[ValidateNever]
		public List<SelectListItem> ManagerList { get; set; }
        public int PositionId { get; set; }
        public string? PositionName { get; set; }
		[ValidateNever]
		public List<SelectListItem> PositionList { get; set; } = new List<SelectListItem>();

		public bool CanApprove { get; set; }
        public int ApprovalLevel { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string Language { get; set; }
        public string TimeZone { get; set; }
        public string Theme { get; set; }
        public string NotificationPreferences { get; set; }
    }
}
