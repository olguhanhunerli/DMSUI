using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Complaints
{
    public class CreateComplaintDTO
    {
        public int companyId { get; set; }
        public int customerId { get; set; }
        public int channelId { get; set; }
        public int typeId { get; set; }
        public int severityId { get; set; }

        public string title { get; set; } = "";
        public string description { get; set; } = "";

        public bool isRepeat { get; set; }
        public bool interimActionRequired { get; set; }
        public string? interimActionNote { get; set; }

        public int assignedTo { get; set; }
    }
}
