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
        Task<PagedResultDTO<DocumentListDTO>> GetPagedAsync(int page, int pageSize, int roleId, int departmentId);
        Task<PagedResultDTO<DocumentListDTO>> GetPagedRejectAsync(int page, int pageSize);
       
        Task<DocumentCreatePreviewDTO> GetDocumentCreatePreview(int categoryId);
        Task<DocumentDetailDTO> GetByIdAsync(int documentId);
		Task<PdfFileResultDTO?> GetPdfAsync(int documentId);
        Task<DownloadFileResult> DownloadOriginalAsync(int documentId);
        Task<DownloadFileResult> DownloadPdfAsync(int documentId);
        Task<PagedResultDTO<DocumentListDTO>> GetDocumentsByCategoryAsync(int page, int pageSize, int categoryId);
	}
}
