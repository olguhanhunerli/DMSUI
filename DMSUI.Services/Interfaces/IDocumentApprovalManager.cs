using DMSUI.Entities.DTOs.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IDocumentApprovalManager
    {
        Task<List<MyPendingDocumentDTO>> GetMyPendingApprovalAsync(int page, int pageSize);
    }
}
