using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.User
{
    public class UserUpdateDTO
    {
		public int Id { get; set; }

		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;

		public int RoleId { get; set; }
		public int DepartmentId { get; set; }
		public int CompanyId { get; set; }

		public int? ManagerId { get; set; }
		public int PositionId { get; set; }

		public bool CanApprove { get; set; }
		public int ApprovalLevel { get; set; }

		public bool IsActive { get; set; }
		public bool IsLocked { get; set; }

		public string Language { get; set; } = string.Empty;
		public string TimeZone { get; set; } = string.Empty;
		public string Theme { get; set; } = string.Empty;
		public string NotificationPreferences { get; set; } = string.Empty;
	}
}
