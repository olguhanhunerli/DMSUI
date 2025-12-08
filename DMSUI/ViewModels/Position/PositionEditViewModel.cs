using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.Position
{
	public class PositionEditViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int CompanyId { get; set; }
		public string? Description { get; set; }
		public bool IsActive { get; set; }

		public List<SelectListItem> CompanyList { get; set; } = new();
	}
}
