using DMSUI.Entities.DTOs.Audit;
using DMSUI.Entities.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface IAuditApiClient
    {
        Task<PagedResultDTO<DocumentAccessLogDTO>> GetDocumentAccessLogsAsync(int page, int pageSize);
    }
}
