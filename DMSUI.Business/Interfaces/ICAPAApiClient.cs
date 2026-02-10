using DMSUI.Entities.DTOs.CAPA;
using DMSUI.Entities.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface ICAPAApiClient
    {
        Task<PagedResultDTO<CAPADTO>> GetAllCAPAS(int page, int pageSize);
        Task<CAPADetailDTO> GetCAPASByCapaNo(string capaNo);
        Task<CAPACreateFormDTO> CreateFormCAPAS(string complaintNo);
        Task<CAPADTO> CreateCAPAAsync(CAPACreateReqDTO dto);
        Task<bool> UpdateCAPAAsync(string capaNo, CAPAUpdateReqDTO dto);
        Task<bool> ClosedCapaAsync(string capaNo, CloseCapaDTO dto);
    }
}
