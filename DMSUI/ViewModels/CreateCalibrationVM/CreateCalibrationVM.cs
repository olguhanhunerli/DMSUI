using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.CreateCalibrationVM
{
	public class CreateCalibrationVM
	{
		public ulong InstrumentId { get; set; }

		public DateTime CalibrationDate { get; set; } = DateTime.Today;
		public int IntervalMonths { get; set; } = 12;

		public string Result { get; set; } = "PASS";

		public string? CalibrationCompany { get; set; }
		public string? CertificateNo { get; set; }

		public int CompanyId { get; set; }
		public string? Location { get; set; }
		public List<SelectListItem> Instruments
		{
			get; set;
		} = new();
}
}
