using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CAPA
{
    public class CAPACreateReqDTO
    {

        public string ComplaintNo { get; set; } = "";
        public string Nonconformity { get; set; } = "";
        public int RootCauseMethodId { get; set; }
        public string RootCause { get; set; } = "";
        public string CorrectiveAction { get; set; } = "";
        public string Status { get; set; } = "";
        public int OwnerId { get; set; }
        public int CompanyId { get; set; }
        public DateTime DueDate { get; set; }

        public string? EffectivenessCheck { get; set; }
        public int? EffectivenessCheckedBy { get; set; }
    }
}
