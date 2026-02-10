using DMSUI.Entities.DTOs.CapaActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface ICapaActionsApiClient
    {
        Task<CapaActionDTO> CreateCapaActionAsync(string capaNo, CreateCapaActionDTO dto);
        Task<bool> ComplateActionAsync(int actionId, string status);
    }
}
