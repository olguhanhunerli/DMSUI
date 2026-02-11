using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.CapaActions;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class CapaActionsManager : ICapaActionsManager
    {
        private readonly ICapaActionsApiClient _apiClient;

        public CapaActionsManager(ICapaActionsApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> ComplateActionAsync(int actionId, string status)
        {
            return await _apiClient.ComplateActionAsync(actionId, status);
        }

        public async Task<List<CapaActionDTO>> GetCapaActionAsync(string capaNo)
        {
            return await _apiClient.GetCapaActionAsync(capaNo);
        }

        public async Task<CapaActionDTO> CreateCapaActionAsync(string capaNo, CreateCapaActionDTO dto)
        {
            return await _apiClient.CreateCapaActionAsync(capaNo, dto);
        }
    }
}
