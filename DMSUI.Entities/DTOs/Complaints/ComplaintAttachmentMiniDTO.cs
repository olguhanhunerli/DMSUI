using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Complaints
{
    public class ComplaintAttachmentMiniDTO
    {
		public int Id { get; set; }
		public string ComplaintNo { get; set; }
		public string OriginalFileName { get; set; }
		public int FileSize { get; set; }
		public DateTime UploadedAt { get; set; }
		public int UploadedBy { get; set; }
		public string UploadedByName { get; set; }
		public bool? IsDeleted { get; set; }
	}
}
