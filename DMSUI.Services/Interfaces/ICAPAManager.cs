using DMSUI.Entities.DTOs.CAPA;
using DMSUI.Entities.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface ICAPAManager
    {
        Task<PagedResultDTO<CAPADTO>> GetAllCAPAS(int page, int pageSize);
        Task<CAPADetailDTO> GetCAPASByCapaNo(string capaNo);
        Task<CAPACreateFormDTO> CreateFormCAPAS(string complaintNo);
        Task<CAPADTO> CreateCAPAAsync(CAPACreateReqDTO dto);
    }
}
