using DMSUI.Business.Interfaces;
using DMSUI.Controllers;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Document;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<CreateDocumentResponseDTO> CreateAsync(CreateDocumentDTO dto)
        {
            return await _client.CreateAsync(dto);
        }

		public async Task<DocumentDetailDTO> GetByIdAsync(int documentId)
		{
			return await _client.GetByIdAsync(documentId);
		}

		public async Task<DocumentCreatePreviewDTO> GetDocumentCreatePreview(int categoryId)
		{
			return await _client.GetDocumentCreatePreview(categoryId);
		}

		public async Task<PagedResultDTO<DocumentListDTO>> GetPagedAsync(int page, int pageSize)
        {
            return await _client.GetPagedAsync(page, pageSize);
        }

		public async Task<PdfFileResultDTO?> GetPdfAsync(int documentId)
		{
			return await _client.GetPdfAsync(documentId);
		}
	}
}
