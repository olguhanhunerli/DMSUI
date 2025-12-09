using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface IPositionApiClient
    {
        Task<List<PositionListDTO>> GetAllPositionsAsync();
		Task<PositionDetailDTO> GetPositionByIdAsync(int id);
        Task<PagedResultDTO<PositionListDTO>> GetPagedAsync(int page, int pageSize);
        Task<bool> AddPositionAsync(PositionCreateDTO positionCreateDTO);
		Task<bool> DeletePositionAsync(int id);
		Task<bool> UpdatePositionAsync(PositionUpdateDTO positionUpdateDTO, int id);
	}
}
