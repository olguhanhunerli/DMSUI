using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.Department
{
	public class DepartmentCreateViewModel
	{
		public string Name { get; set; }
		public string DepartmentCode { get; set; }
		public int CompanyId { get; set; }
		public int ManagerId { get; set; }
		public string Description { get; set; }
		public string Notes { get; set; }
		public int SortOrder { get; set; }
		public List<SelectListItem> CompanySelectList { get; set; } = new();
		public List<SelectListItem> ManagerSelectList { get; set; } = new();
	}
}
