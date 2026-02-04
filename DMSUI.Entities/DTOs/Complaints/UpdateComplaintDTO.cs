using DMSUI.Entities.DTOs.Assignees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Complaints
{
    public class UpdateComplaintDTO
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

		public string? partNumber { get; set; }
		public string? partRevision { get; set; }
		public string? lotNumber { get; set; }
		public string? serialNumber { get; set; }
		public DateTime? productionDate { get; set; }
		public string? productionLine { get; set; }

		public string? customerComplaintNo { get; set; }
		public string? customerPO { get; set; }
		public string? deliveryNoteNo { get; set; }

		public int? quantityAffected { get; set; }
		public string? containmentAction { get; set; }
        public List<AssigneeDTO> assignees { get; set; } = new();
    }
}
