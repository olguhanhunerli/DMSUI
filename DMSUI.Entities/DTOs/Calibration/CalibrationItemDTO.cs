using DMSUI.Entities.DTOs.CalibrationFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Calibration
{
    public class CalibrationItemDTO
    {
        public int CalibrationId { get; set; }
        public int InstrumentId { get; set; }

        public string AssetCode { get; set; } = string.Empty;
        public string InstrumentName { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;

        public string InstrumentLocation { get; set; } = string.Empty;

        public DateTime CalibrationDate { get; set; }
        public DateTime DueDate { get; set; }
        public int IntervalMonths { get; set; }

        public string Result { get; set; } = string.Empty;
        public string CalibrationCompany { get; set; } = string.Empty;
        public string CertificateNo { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public int RemainingDays { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;

        public int? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<CalibrationFileDTO> Files { get; set; } = new();
    }
}