using DMSUI.ViewModels.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.Department
{
	public class DepartmentEditViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string DepartmentCode { get; set; }

		public int? ManagerId { get; set; }
		public int CompanyId { get; set; }	
		public string CompanyName { get; set; }

		public string? Description { get; set; }
		public string? Notes { get; set; }
		public int? SortOrder { get; set; }

		public List<UserMiniViewModel> Users { get; set; } = new();
		public List<SelectListItem> ManagerSelectList { get; set; } = new();
	}
}
