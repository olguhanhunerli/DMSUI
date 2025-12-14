using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class DocumentAccessLogDTO
    {
		public int Id { get; set; }
		public int DocumentId { get; set; }

		public int UserId { get; set; }
		public string? UserName { get; set; }

		public string? AccessType { get; set; }
		public DateTime? AccessAt { get; set; }
		public string? IpAddress { get; set; }

		public string? AccessTypeDisplayName { get; set; }
		public string? AccessTimeText { get; set; }
	}
}
