using DMSUI.Entities.DTOs.CapaActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface ICapaActionsManager
    {
        Task<CapaActionDTO> CreateCapaActionAsync(string capaNo, CreateCapaActionDTO dto);
        Task<bool> ComplateActionAsync(int actionId, string status);
        Task<List<CapaActionDTO>> GetCapaActionAsync(string capaNo);
    }
}
