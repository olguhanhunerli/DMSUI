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
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("serial_No")]
        public string? Serial_No { get; set; }

        [JsonPropertyName("measurement_Range")]
        public string? Measurement_Range { get; set; }

        [JsonPropertyName("resolution")]
        public string? Resolution { get; set; }

        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [JsonPropertyName("instrument_Type")]
        public string? Instrument_Type { get; set; }

        [JsonPropertyName("measurement_Discipline")]
        public string? Measurement_Discipline { get; set; }

        [JsonPropertyName("is_Critical")]
        public bool? Is_Critical { get; set; }

        [JsonPropertyName("risk_Level")]
        public string? Risk_Level { get; set; }

        [JsonPropertyName("measurement_Uncertainty")]
        public string? Measurement_Uncertainty { get; set; }

        [JsonPropertyName("environment_Required")]
        public bool? Environment_Required { get; set; }

        [JsonPropertyName("environment_Notes")]
        public string? Environment_Notes { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("owner_Person")]
        public string? Owner_Person { get; set; }
    }
}
