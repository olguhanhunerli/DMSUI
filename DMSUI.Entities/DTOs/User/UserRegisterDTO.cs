using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.User
{
    public class UserRegisterDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

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
    }
}
