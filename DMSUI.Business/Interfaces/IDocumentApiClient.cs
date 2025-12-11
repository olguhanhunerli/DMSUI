using DMSUI.Controllers;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface IDocumentApiClient
    {
        Task<CreateDocumentResponseDTO> CreateAsync(CreateDocumentDTO dto);
        Task<PagedResultDTO<DocumentListDTO>> GetPagedAsync(int page, int pageSize);
        Task<DocumentCreatePreviewDTO> GetDocumentCreatePreview(int categoryId);
    }
}
