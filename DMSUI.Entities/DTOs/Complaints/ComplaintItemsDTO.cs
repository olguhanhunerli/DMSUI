using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Complaints
{
    public class ComplaintItemsDTO
    {
        public int id { get; set; }
        public string complaintNo { get; set; }
        public string companyName { get; set; }
        public int customerId { get; set; }
        public string customerName { get; set; }
        public int channelId { get; set; }
        public int typeId { get; set; }
        public int severityId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool isRepeat { get; set; }
        public bool needsCapa { get; set; }
        public bool? interimActionRequired { get; set; }
        public string interimActionNote { get; set; }
        public bool? isClosed { get; set; }
        public bool? isDeleted { get; set; }
        public string status { get; set; }
        public DateTime reportedAt { get; set; }
        public int? createdBy { get; set; }
        public string? createdByName { get; set; }
        public string? closedByName { get; set; }
        public int? assignedTo { get; set; }
        public string? assignedToName { get; set; }
        public int? deletedBy { get; set; }
        public string deletedByName { get; set; }
        public int? updateBy { get; set; }
        public string? updateByName { get; set; }
        public DateTime? closedAt { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }
    }
}
