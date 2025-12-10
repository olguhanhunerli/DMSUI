using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class CreateDocumentAttachmentDTO
    {
        public int DocumentId { get; set; }
        public List<IFormFile> Files { get; set; } = new();
        public bool IsMainFile { get; set; } = false;
    }
}
