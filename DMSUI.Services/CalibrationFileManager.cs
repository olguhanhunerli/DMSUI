using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.CalibrationFile;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
	public class CalibrationFileManager : ICalibrationFileManager
	{
		private readonly ICalibrationFileApiClient _calibrationFileApiClient;

		public CalibrationFileManager(ICalibrationFileApiClient calibrationFileApiClient)
		{
			_calibrationFileApiClient = calibrationFileApiClient;
		}

		public async Task<bool> UploadCalibrationFilesAsync(UploadCalibrationFileDTO dto)
		{
			return await _calibrationFileApiClient.UploadCalibrationFilesAsync(dto);
		}
	}
}
