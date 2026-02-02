using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Entities.DTOs.Customers;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class ComplaintManager : IComplaintManager
    {
        private readonly IComplaintApiClient _complaintApiClient;

        public ComplaintManager(IComplaintApiClient complaintApiClient)
        {
            _complaintApiClient = complaintApiClient;
        }

		public async Task<bool> ClosedComplaint(string complaintNo)
		{
			return await _complaintApiClient.ClosedComplaint(complaintNo);
		}

		public async Task<string?> CreateComplaint(CreateComplaintDTO complaint)
		{
			var created = await _complaintApiClient.CreateComplaint(complaint);
			return created?.complaintNo;
        }

		public async Task<ComplaintAttachmentMiniDTO> CreateComplaintAttachment(string complaintNo, IFormFile file)
		{
			return await _complaintApiClient.CreateComplaintAttachment(complaintNo, file);
		}

		public async Task<bool> DeleteComplaintAttachment(int fileId)
		{
			return await _complaintApiClient.DeleteComplaintAttachment(fileId);
		}

		public async Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadComplaintFilesAsync(int fileId)
		{
			return await _complaintApiClient.DownloadComplaintFilesAsync(fileId);
		}

		public async Task<List<ComplaintAttachmentMiniDTO>> GetComplaintAttachment(string complaintNo)
		{
			return await _complaintApiClient.GetComplaintAttachment(complaintNo);
		}

		public async Task<ComplaintItemsDTO> GetComplaintById(string complaintNo)
		{
			return await _complaintApiClient.GetComplaintById(complaintNo);
		}

        public async Task<List<ComplaintForCapaSelectDTO>> GetComplaintForCapaSelect(string? search, int take)
        {
           return await _complaintApiClient.GetComplaintForCapaSelect(search, take);
        }

        public async Task<PagedResultDTO<ComplaintItemsDTO>> GetComplaintsPaging(int page, int pageSize)
        {
           return await _complaintApiClient.GetComplaintsPaging(page, pageSize);
        }

		public async Task<string> UpdateComplaint(string complaintNo, UpdateComplaintDTO dto)
		{
			var updated = await _complaintApiClient.UpdateComplaint(complaintNo, dto);
			return updated?.complaintNo;
		}
	}
}
