using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Instruments
{
    public class CreateInstrumentDTO
    {
		[JsonPropertyName("companyId")]
		public int CompanyId { get; set; }

		[JsonPropertyName("asset_Code")]
		public string Asset_Code { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("brand")]
		public string Brand { get; set; }

		[JsonPropertyName("model")]
		public string Model { get; set; }

		[JsonPropertyName("serial_No")]
		public string Serial_No { get; set; }

		[JsonPropertyName("measurement_Range")]
		public string Measurement_Range { get; set; }

		[JsonPropertyName("resolution")]
		public string Resolution { get; set; }

		[JsonPropertyName("unit")]
		public string Unit { get; set; }

		[JsonPropertyName("location")]
		public string Location { get; set; }

		[JsonPropertyName("owner_Person")]
		public string Owner_Person { get; set; }

	}
}
