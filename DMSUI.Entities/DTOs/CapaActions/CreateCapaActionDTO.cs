using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CapaActions
{
    public class CreateCapaActionDTO
    {
        public string ActionType { get; set; } = "";
        public string Description { get; set; } = "";
        public int OwnerId { get; set; }
        public DateTime DueDate { get; set; }
        public bool EvidenceRequired { get; set; }
    }
}
