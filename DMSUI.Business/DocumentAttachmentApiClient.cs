using DMSUI.Entities.DTOs.Document;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
	public class DocumentAttachmentApiClient : IDocumentAttachmentApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentAttachmentApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        private void AttachToken()
        {
            var token = _httpContextAccessor.HttpContext?
                .Request.Cookies["access_token"];

            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task UploadMultipleAsync(int documentId, List<IFormFile> files)
        {
            AttachToken();
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(documentId.ToString()), "DocumentId");
            content.Add(new StringContent("false"), "IsMainFile");

            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                var fileContent = new StreamContent(stream);

                content.Add(fileContent, "Files", file.Name);
            }

            var response = await _httpClient.PostAsync("api/DocumentAttachment/upload-multiple", content);
            response.EnsureSuccessStatusCode();
        }

		public async Task<DownloadFileResult> DownloadAttachmentAsync(int attachmentId)
		{
            AttachToken();
			var response = await _httpClient.GetAsync(
	              $"api/DocumentAttachment/download/{attachmentId}",
	              HttpCompletionOption.ResponseHeadersRead 
              );

			response.EnsureSuccessStatusCode();

			var stream = await response.Content.ReadAsStreamAsync();

			var contentType =
				response.Content.Headers.ContentType?.ToString()
				?? "application/octet-stream";

			var fileName =
				response.Content.Headers.ContentDisposition?.FileNameStar
				?? response.Content.Headers.ContentDisposition?.FileName
				?? "attachment";

			return new DownloadFileResult
			{
				Stream = stream,
				ContentType = contentType,
				FileName = fileName.Replace("\"", "")
			};
		}
	}
}
