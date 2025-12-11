using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Approval
{
    public class RejectDocumentDTO
    {
        public int DocumentId { get; set; }
        public string Reason { get; set; } = null;
    }
}
