using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CAPA
{
    public class CapaActionListDTO
    {
        public int Id { get; set; }
        public string CapaNo { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }

        public int OwnerId { get; set; }
        public string OwnerName { get; set; }

        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public bool EvidenceRequired { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
