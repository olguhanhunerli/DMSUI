using DMSUI.Entities.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Departments
{
    public class DepartmentEditDTO
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? DepartmentCode { get; set; }
		public int? ManagerId { get; set; }
		public string? ManagerName { get; set; }
		public int? CompanyId { get; set; }
		public string? CompanyName { get; set; }
		public int CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public int UploadedBy { get; set; }
		public string UploadedByName { get; set; }
		public List<UserListDTO> Users { get; set; }
		public List<UserListDTO> ManagerList { get; set; }
	}
}
