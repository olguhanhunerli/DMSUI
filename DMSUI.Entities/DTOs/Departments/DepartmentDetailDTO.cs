using DMSUI.Entities.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Departments
{
    public class DepartmentDetailDTO
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? DepartmentCode { get; set; }
		public int? ManagerId { get; set; }
		public string? ManagerName { get; set; }

		public int? CompanyId { get; set; }
		public string? CompanyName { get; set; }

		public string? Description { get; set; }
		public string? Notes { get; set; }
		public int? SortOrder { get; set; }

		public int? CreatedBy { get; set; }
		public int? UploadedBy { get; set; }

		public List<UserMiniDTO> Users { get; set; } = new();
	}
}
