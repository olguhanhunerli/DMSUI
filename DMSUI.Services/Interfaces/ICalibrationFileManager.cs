using DMSUI.Entities.DTOs.CalibrationFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface ICalibrationFileManager
    {
		Task<bool> UploadCalibrationFilesAsync(UploadCalibrationFileDTO dto);
		Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadCalibrationFilesAsync(int fileId, bool asPdf);
		Task<bool> DeleteFiles(int fileId);
	}
}
