using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.CAPA;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class CAPAManager : ICAPAManager
    {
        private readonly ICAPAApiClient _apiClient;

        public CAPAManager(ICAPAApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CAPACreateFormDTO> CreateFormCAPAS(string complaintNo)
        {
            return await _apiClient.CreateFormCAPAS(complaintNo);
        }

        public async Task<PagedResultDTO<CAPADTO>> GetAllCAPAS(int page, int pageSize)
        {
            return await _apiClient.GetAllCAPAS(page, pageSize);
        }

        public async Task<CAPADetailDTO> GetCAPASByCapaNo(string capaNo)
        {
            return await _apiClient.GetCAPASByCapaNo(capaNo);
        }
    }
}
