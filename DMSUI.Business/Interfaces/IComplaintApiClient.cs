using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Complaints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface IComplaintApiClient
    {
        Task<PagedResultDTO<ComplaintItemsDTO>> GetComplaintsPaging(int page, int pageSize);
        Task<ComplaintItemsDTO> CreateComplaint(CreateComplaintDTO complaint);
        Task<ComplaintItemsDTO> GetComplaintById(string complaintNo);
        Task<ComplaintItemsDTO> UpdateComplaint(string complaintNo, UpdateComplaintDTO dto);
		Task<bool> ClosedComplaint(string complaintNo);
        Task<ComplaintAttachmentMiniDTO> CreateComplaintAttachment(string complaintNo, IFormFile file);
        Task<List<ComplaintAttachmentMiniDTO>> GetComplaintAttachment(string complaintNo);
		Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadComplaintFilesAsync(int fileId);
        Task<bool> DeleteComplaintAttachment(int fileId);
	}
}
