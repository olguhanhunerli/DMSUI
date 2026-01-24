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
	}
}
