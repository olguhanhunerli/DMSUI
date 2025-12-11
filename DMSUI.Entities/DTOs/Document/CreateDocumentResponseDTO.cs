using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class CreateDocumentResponseDTO
    {
        public int Id { get; set; }
        public string DocumentCode { get; set; }
        public int StatusId { get; set; }

        public string Status { get; set; }   
        public int VersionNumber { get; set; }
        public int RevisionNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Message { get; set; }
    }
}
