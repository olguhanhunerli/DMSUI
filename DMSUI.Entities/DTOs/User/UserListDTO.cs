using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.User
{
    public class UserListDTO
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; } = string.Empty;

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
		public int? ManagerId { get; set; }
		public string? ManagerName { get; set; }

        public int PositionId { get; set; }
        public string? PositionName { get; set; }

        public bool CanApprove { get; set; }
        public int ApprovalLevel { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string Language { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;

        public string NotificationPreferences { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";
    }
}
