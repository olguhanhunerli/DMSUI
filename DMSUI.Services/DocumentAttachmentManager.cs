using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Document;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
	public class DocumentAttachmentManager : IDocumentAttachmentManager
    {
        private readonly IDocumentAttachmentApiClient _client;

        public DocumentAttachmentManager(IDocumentAttachmentApiClient client)
        {
            _client = client;
        }

		public async Task<DownloadFileResult> DownloadAttachmentAsync(int attachmentId)
		{
			return await _client.DownloadAttachmentAsync(attachmentId);
		}

		public async Task UploadMultipleAsync(int documentId, List<IFormFile> files)
        {
             await _client.UploadMultipleAsync(documentId, files);
        }
    }
}
