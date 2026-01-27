using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CalibrationFile
{
    public class UploadCalibrationFileDTO
    {
        public long CalibrationId { get; set; }

        public IFormFile File { get; set; } = null!; // ZORUNLU

        public string? InstrumentName { get; set; }
        public string? FileType { get; set; }
        public string? Description { get; set; }
    }
}