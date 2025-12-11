using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Approval;
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

        public async Task ApproveAsync(int documentId)
        {
             await _client.ApproveAsync(documentId);
        }

        public async Task<List<MyPendingDocumentDTO>> GetMyPendingApprovalAsync(int page, int pageSize)
        {
            return await _client.GetMyPendingApprovalAsync(page, pageSize);
        }

        public async Task InitApprovalAsync(CreateDocumentApprovalDTO createDocumentApprovalDTO)
        {
             await _client.InitApprovalAsync(createDocumentApprovalDTO);
        }

        public async Task RejectAsync(int documentId, string reason)
        {
            await _client.RejectAsync(documentId, reason);
        }
    }
}
