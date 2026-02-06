using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Complaints
{
    public class ComplaintMiniDTO
    {
        public int Id { get; set; }
        public string? ComplaintNo { get; set; }
        public string? CompanyName { get; set; }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public int SeverityId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public DateTime ReportedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string? PartNumber { get; set; }
        public string? PartRevision { get; set; }
        public string? LotNumber { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? ProductionDate { get; set; }
        public string? ProductionLine { get; set; }

        public bool NeedsCapa { get; set; }
        public bool InterimActionRequired { get; set; }
        public string? InterimActionNote { get; set; }
        public string? Status { get; set; }
        public bool IsClosed { get; set; }
        public string? CustomerComplaintNo { get; set; }
        public string? CustomerPO { get; set; }
    }
}
