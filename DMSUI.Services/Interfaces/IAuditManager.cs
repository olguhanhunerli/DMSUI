using DMSUI.Entities.DTOs.Audit;
using DMSUI.Entities.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IAuditManager
    {
        Task<PagedResultDTO<DocumentAccessLogDTO>> GetDocumentAccessLogsAsync(int page, int pageSize);
    }
}
