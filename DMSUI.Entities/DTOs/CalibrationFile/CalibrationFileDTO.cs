using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CalibrationFile
{
    public class CalibrationFileDTO
	{
		public long FileId { get; set; }
		public long CalibrationId { get; set; }

		public string FileOriginalName { get; set; } = default!;
		public string FilePath { get; set; } = default!;

		public string? PdfFilePath { get; set; }
		public string? FileMime { get; set; }
		public long? FileSize { get; set; }
		public string? FileType { get; set; }
		public string? Description { get; set; }
		public string CreatedByName { get; set; }
		public DateTime CreatedAt { get; set; }
		public string UploadedByName { get; set; }
		public DateTime UpdatedAt { get; set; }

	}
}
