using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Position
{
    public class PositionListDTO
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int? CompanyId { get; set; }
		public string? CompanyName { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreatedAt { get; set; }
		public int? CreatedBy { get; set; }
	}
}
