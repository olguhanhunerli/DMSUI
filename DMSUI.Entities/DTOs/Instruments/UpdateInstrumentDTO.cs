using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Instruments
{
    public class UpdateInstrumentDTO
    {
		public string Name { get; set; } = string.Empty;

		public string? Brand { get; set; }

		public string? Model { get; set; }

		public string? Serial_No { get; set; }

		public string? Measurement_Range { get; set; }

		public string? Resolution { get; set; }

		public string? Unit { get; set; }

		public string? Location { get; set; }

		public string? Owner_Person { get; set; }
	}
}
