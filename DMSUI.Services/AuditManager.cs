using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Audit;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class AuditManager : IAuditManager
    {
        private readonly IAuditApiClient _auditApiClient;

        public AuditManager(IAuditApiClient auditApiClient)
        {
            _auditApiClient = auditApiClient;
        }

        public async Task<PagedResultDTO<DocumentAccessLogDTO>> GetDocumentAccessLogsAsync(int page, int pageSize)
        {
            return await _auditApiClient.GetDocumentAccessLogsAsync(page, pageSize);
        }
    }
}
