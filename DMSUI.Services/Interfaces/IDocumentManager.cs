using DMSUI.Controllers;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IDocumentManager
    {
        Task<CreateDocumentResponseDTO> CreateAsync(CreateDocumentDTO dto);
		Task<PagedResultDTO<DocumentListDTO>> GetPagedAsync(int page, int pageSize);
		Task<DocumentCreatePreviewDTO> GetDocumentCreatePreview(int categoryId);
		Task<DocumentDetailDTO> GetByIdAsync(int documentId);
		Task<PdfFileResultDTO?> GetPdfAsync(int documentId);
	}
}
