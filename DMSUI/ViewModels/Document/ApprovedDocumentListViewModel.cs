using DMSUI.Entities.DTOs.Document;

namespace DMSUI.ViewModels.Document
{
	public class ApprovedDocumentListViewModel
	{
		public List<DocumentListDTO> Documents { get; set; } = new();

		public int Page { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }

		public int TotalPages =>
			(int)Math.Ceiling((double)TotalCount / PageSize);
	}
}
