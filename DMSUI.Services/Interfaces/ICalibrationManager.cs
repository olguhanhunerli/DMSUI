using DMSUI.Entities.DTOs.Calibration;
using DMSUI.Entities.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface ICalibrationManager
    {
		Task<PagedResultDTO<CalibrationItemDTO>> GetCalibrationItemsAsync(int pageNumber, int pageSize);
		Task<CalibrationItemDTO> GetCalibrationItemByIdAsync(int calibrationId);
		Task<ulong?> CreateCalibrationAsync(CreateCalibrationDTO createCalibrationDTO);

		Task<bool> UpdateCalibrationAsync(ulong id, EditCalibrationDTO editCalibrationDTO);

		Task<bool> DeleteByIdAsync(int calibrationId);
	}
}
