using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CapaActions
{
    public class CapaActionDTO
    {
        public int Id { get; set; }

        public string CapaNo { get; set; } = string.Empty;

        public string ActionType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int OwnerId { get; set; }

        public string? OwnerName { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public bool EvidenceRequired { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
