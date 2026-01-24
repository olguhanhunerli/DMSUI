using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Calibration
{
    public class CreateCalibrationDTO
    {
		public ulong InstrumentId { get; set; }

		public DateTime CalibrationDate { get; set; } = DateTime.Today;
		public int IntervalMonths { get; set; } = 12;

		public string Result { get; set; } = "PASS";

		public string? CalibrationCompany { get; set; }
		public string? CertificateNo { get; set; }

		public int CompanyId { get; set; }
		public string? Location { get; set; }
	}
}
