using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Exceptions
{
    public class APIErrorDTO
    {
		public string? Title { get; set; }
		public string? Message { get; set; }
		public string? Detail { get; set; }
		public int? Status { get; set; }
	}
}
