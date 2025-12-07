using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Position
{
    public class PositionCreateDTO
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public int CompanyId { get; set; }
		public bool IsActive { get; set; }
		public int CreatedBy { get; set; }
	}
}
