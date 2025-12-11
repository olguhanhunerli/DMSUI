using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.Document
{
	public class DocumentCreatePreviewViewModel
	{

		public string DocumentCode { get; set; }
        public int CategoryId { get; set; }
        public string CompanyName { get; set; }
		public string CategoryName { get; set; }
		public string CategoryBreadcrumb { get; set; }
		public int VersionNumber { get; set; }
		public bool IsCodeValid { get; set; }

		public int PreparedByUserId { get; set; }
		public string PreparedByUserName { get; set; }
		public DateTime PreparedAt { get; set; }

		public string? TitleTr { get; set; }
		public string? TitleEn { get; set; }
		public string? DocumentType { get; set; }
		public int RevisionNumber { get; set; } = 0;
		public int? DepartmentId { get; set; }

		public IFormFile? DocumentFile { get; set; }

		public List<SelectListItem> DepartmentList { get; set; } = new();
		public List<ApprovalRowViewModel> ApprovalList { get; set; } = new();
        public List<IFormFile> AttachmentFiles { get; set; } = new();
        public string VersionNote { get; internal set; }
    }
}
