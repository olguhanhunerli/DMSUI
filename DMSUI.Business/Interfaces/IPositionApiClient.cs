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
		Task<bool> AddPositionAsync(PositionCreateDTO positionCreateDTO);
	}
}
