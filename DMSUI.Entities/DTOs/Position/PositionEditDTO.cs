using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Position
{
    public class PositionEditDTO
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int CompanyId { get; set; }

		public string? Description { get; set; }
		public string? Notes { get; set; }
		public int? SortOrder { get; set; }
		public bool? IsActive { get; set; }
	}
}
