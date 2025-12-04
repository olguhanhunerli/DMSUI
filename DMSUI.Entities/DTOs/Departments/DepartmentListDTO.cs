using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Departments
{
    public class DepartmentListDTO
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? DepartmentCode { get; set; }
		public int? ManagerId { get; set; }
		public string? ManagerName { get; set; }
		public int? CompanyId { get; set; }
		public string? CompanyName { get; set; }
	}
}
