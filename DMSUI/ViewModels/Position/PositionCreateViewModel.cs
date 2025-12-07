using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.Position
{
	public class PositionCreateViewModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int CompanyId { get; set; }
		public bool IsActive { get; set; }
		public int CreatedBy { get; set; }
		public List<SelectListItem> CompanyList { get; set; } = new();
	}
}
