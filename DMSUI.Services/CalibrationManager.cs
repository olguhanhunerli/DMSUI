using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Calibration;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
	public class CalibrationManager : ICalibrationManager
	{
		private readonly ICalibrationApiClient _apiClient;

		public CalibrationManager(ICalibrationApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public async Task<ulong?> CreateCalibrationAsync(CreateCalibrationDTO createCalibrationDTO)
		{
			return await _apiClient.CreateCalibrationAsync(createCalibrationDTO);
		}

		public async Task<bool> DeleteByIdAsync(int calibrationId)
		{
			return await _apiClient.DeleteByIdAsync(calibrationId);
		}

		public async Task<CalibrationItemDTO> GetCalibrationItemByIdAsync(int calibrationId)
		{
			return await _apiClient.GetCalibrationItemByIdAsync(calibrationId);
		}

		public async Task<PagedResultDTO<CalibrationItemDTO>> GetCalibrationItemsAsync(int pageNumber, int pageSize)
		{
			return await _apiClient.GetCalibrationItemsAsync(pageNumber, pageSize);
		}

		public async Task<bool> UpdateCalibrationAsync(int id, EditCalibrationDTO editCalibrationDTO)
		{
			return await _apiClient.UpdateCalibrationAsync(id, editCalibrationDTO);
		}
	}
}
