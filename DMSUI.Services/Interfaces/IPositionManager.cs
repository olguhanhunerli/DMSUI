using DMSUI.Entities.DTOs.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IPositionManager
    {
        Task<List<PositionListDTO>> GetAllPositionsAsync();
        Task<bool> AddPositionAsync(PositionCreateDTO position);
        Task<PositionDetailDTO> GetPositionByIdAsync(int id);
        Task<bool> DeletePositionAsync(int id);
        Task<bool> UpdatePositionAsync(PositionUpdateDTO positionUpdateDTO, int id);
    }
}
