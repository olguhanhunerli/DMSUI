using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class DocumentAttachmentDTO
    {
		public int Id { get; set; }
		public int DocumentId { get; set; }

		public string FileName { get; set; } = null!;
		public string OriginalFileName { get; set; } = null!;
		public long FileSize { get; set; }
		public string FileType { get; set; } = null!;
		public string FilePath { get; set; } = null!;

		public DateTime? UploadedAt { get; set; }
		public int? UploadedByUserId { get; set; }
		public string? UploadedByName { get; set; }

		public bool IsMainFile { get; set; }
		public bool IsDeleted { get; set; }

		public string FileSizeText { get; set; } = "";
	}
}
