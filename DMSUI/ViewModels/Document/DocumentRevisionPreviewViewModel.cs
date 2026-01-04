using DMSUI.Entities.DTOs.Document;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.Document
{
	public class DocumentRevisionPreviewViewModel
	{
		public int DocumentId { get; set; }
		public string DocumentCode { get; set; } = null!;
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = null!;
		public string? CategoryBreadcrumb { get; set; }

		public int CompanyId { get; set; }
		public string CompanyName { get; set; } = null!;

		public int VersionNumber { get; set; }
		public string VersionNote { get; set; } = null!;
		public int StatusId { get; set; }
		public string Status { get; set; } = null!;

		public int OwnerUserId { get; set; }
		public string OwnerName { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public bool IsCodeValid { get; set; }
		public bool IsRevision { get; set; }

		public string NewRevisionNote { get; set; } = null!;
		public IFormFile? DocumentFile { get; set; }
		public string Reason { get; set; }
		public List<ApprovalRowViewModel> ApprovalList { get; set; } = new();
		public List<SelectListItem> DepartmentList { get; set; } = new();
		public List<SelectListItem> RoleList { get; set; } = new();
		public List<SelectListItem> UserList { get; set; } = new();
		public List<DocumentAttachmentDTO> Attachments { get; set; }
	}
}
