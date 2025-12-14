using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class PdfFileResultDTO
    {
		public byte[] FileBytes { get; set; }
		public string FileName { get; set; }
	}
}
