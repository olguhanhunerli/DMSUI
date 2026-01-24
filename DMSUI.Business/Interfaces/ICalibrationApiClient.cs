using DMSUI.Entities.DTOs.Calibration;
using DMSUI.Entities.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface ICalibrationApiClient
    {
        Task<PagedResultDTO<CalibrationItemDTO>> GetCalibrationItemsAsync(int pageNumber, int pageSize);
	}
}
