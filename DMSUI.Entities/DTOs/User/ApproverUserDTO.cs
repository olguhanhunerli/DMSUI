using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.User
{
    public class ApproverUserDTO
    {
		public int Id { get; set; }
		public string FullName { get; set; } = null!;
		public string DepartmentName { get; set; } = null!;
		public string PositionName { get; set; } = null!;

	}
}
