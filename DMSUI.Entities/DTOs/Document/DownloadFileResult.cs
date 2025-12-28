using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class DownloadFileResult
    {
		public Stream Stream { get; set; }
		public string ContentType { get; set; }
		public string FileName { get; set; }
	}
}
