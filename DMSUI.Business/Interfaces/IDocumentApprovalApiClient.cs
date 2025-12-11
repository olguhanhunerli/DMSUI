using DMSUI.Entities.DTOs.Approval;
using DMSUI.Entities.DTOs.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface IDocumentApprovalApiClient
    {
        Task<List<MyPendingDocumentDTO>> GetMyPendingApprovalAsync(int page, int pageSize);
        Task InitApprovalAsync(CreateDocumentApprovalDTO createDocumentApprovalDTO);
        Task ApproveAsync(int documentId);
        Task RejectAsync(int documentId, string reason);
    }
}
