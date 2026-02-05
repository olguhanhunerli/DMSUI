using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CAPA
{
    public class CAPADetailDTO
    {
        public int Id { get; set; }

        public string? CapaNo { get; set; }

        public string? ComplaintNo { get; set; }

        public string? NonConformity { get; set; }

        public string? RootCauseMethodName { get; set; }

        public string? RootCause { get; set; }

        public string? CorrectiveAction { get; set; }

        public string? Status { get; set; }

        public int OwnerId { get; set; }

        public int CompanyId { get; set; }

        public string? EffectivenessCheck { get; set; }

        public int? EffectivenessCheckedBy { get; set; }

        public int RemainingDays { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? EffectivenessCheckedAt { get; set; }

        public DateTime OpenedAt { get; set; }

        public DateTime? CloseAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CompanyName { get; set; }

        public string? ComplaintNumber { get; set; }

        public string? OwnerByName { get; set; }

        public string? EffectivenessCheckedByName { get; set; }
    }
}
