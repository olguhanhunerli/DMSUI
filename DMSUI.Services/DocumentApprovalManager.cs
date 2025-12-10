using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Document;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class DocumentApprovalManager : IDocumentApprovalManager
    {
        private readonly IDocumentApprovalApiClient _client;

        public DocumentApprovalManager(IDocumentApprovalApiClient client)
        {
            _client = client;
        }

        public async Task<List<MyPendingDocumentDTO>> GetMyPendingApprovalAsync(int page, int pageSize)
        {
            return await _client.GetMyPendingApprovalAsync(page, pageSize);
        }
    }
}
