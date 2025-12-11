using DMSUI.Entities.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Approval
{
    public class CreateDocumentApprovalDTO
    {
        public int DocumentId { get; set; }
        public List<ApprovalUserDTO> Approvers { get; set; } = new();
    }
}
