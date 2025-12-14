using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class MainDocumentFileDTO
    {
		public int Id { get; set; }
		public int DocumentId { get; set; }

		public string FileName { get; set; } = null!;
		public string OriginalFileName { get; set; } = null!;
		public string FileExtension { get; set; } = null!;
		public long FileSize { get; set; }
		public string FilePath { get; set; } = null!;
		public string PdfFilePath { get; set; } = null!;

		public string FileSizeText { get; set; } = "";
	}
}
