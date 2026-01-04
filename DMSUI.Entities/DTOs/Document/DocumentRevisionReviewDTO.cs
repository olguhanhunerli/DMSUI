using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class DocumentRevisionReviewDTO
    {
		public int DocumentId { get; set; }
		public string DocumentCode { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public string CategoryBreadcrumb { get; set; }
		public int CompanyId { get; set; }
		public string CompanyName { get; set; }
		public int VersionNumber { get; set; }
		public string VersionNote { get; set; }
		public int StatusId { get; set; }
		public string Status { get; set; }
		public int OwnerUserId { get; set; }
		public string OwnerName { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsCodeValid { get; set; }
		public bool IsRevision { get; set; }
		public string RevisionNote { get; set; }
		public IFormFile RevisionFile { get; set; }
		public List<DocumentAttachmentDTO> Attachments { get; set; }
	}
}
