using DMSUI.Business.Interfaces;
using DMSUI.Controllers;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Document;
using DMSUI.Entities.DTOs.Revision;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
	public class DocumentManager : IDocumentManager
    {
        private readonly IDocumentApiClient _client;

        public DocumentManager(IDocumentApiClient client)
        {
            _client = client;
        }

		public async Task<CancelRevisionDTO> CancelRevisionAsync(int documentId, string reason)
		{
			return await _client.CancelRevisionAsync(documentId, reason);
		}

		public async Task<CreateDocumentResponseDTO> CreateAsync(CreateDocumentDTO dto)
        {
            return await _client.CreateAsync(dto);
        }

		public async Task<DownloadFileResult> DownloadOriginalAsync(int documentId)
		{
			return await _client.DownloadOriginalAsync(documentId);
		}

		public async Task<DownloadFileResult> DownloadPdfAsync(int documentId)
		{
			return await _client.DownloadPdfAsync(documentId);
		}

		public async Task<DocumentDetailDTO> GetByIdAsync(int documentId)
		{
			return await _client.GetByIdAsync(documentId);
		}

		public async Task<DocumentCreatePreviewDTO> GetDocumentCreatePreview(int categoryId)
		{
			return await _client.GetDocumentCreatePreview(categoryId);
		}

		public async Task<PagedResultDTO<DocumentListDTO>> GetDocumentsByCategoryAsync(int page, int pageSize, int categoryId)
		{
			return await _client.GetDocumentsByCategoryAsync(page, pageSize, categoryId);
		}

		public async Task<PagedResultDTO<DocumentListDTO>> GetPagedAsync(int page, int pageSize, int roleId, int departmentId)
        {
            return await _client.GetPagedAsync(page, pageSize, roleId, departmentId);
        }

		public async Task<PagedResultDTO<DocumentListDTO>> GetPagedRejectAsync(int page, int pageSize)
		{
			return await _client.GetPagedRejectAsync(page, pageSize);
		}

		public async Task<PdfFileResultDTO?> GetPdfAsync(int documentId)
		{
			return await _client.GetPdfAsync(documentId);
		}

		public async Task<DocumentRevisionReviewDTO> GetRevisionReviewAsync(int documentId)
		{
			return await _client.GetRevisionReviewAsync(documentId);	
		}

		public async Task<PagedResultDTO<MyActiveRevisionDTO>> MyActiveRevisionAsync(int page, int pageSize)
		{
			return await _client.MyActiveRevisionAsync(page, pageSize);
		}

		public async Task<StartRevisionDTO> StartRevisionAsync(int documentId, string revisionNote)
		{
			return await _client.StartRevisionAsync(documentId, revisionNote);
		}
	}
}
