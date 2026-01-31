using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Entities.DTOs.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IComplaintManager
    {
        Task<PagedResultDTO<ComplaintItemsDTO>> GetComplaintsPaging(int page, int pageSize);
        Task<bool> CreateComplaint(CreateComplaintDTO complaint);
		Task<ComplaintItemsDTO> GetComplaintById(string complaintNo);
		Task<bool> UpdateComplaint(string complaintNo, UpdateComplaintDTO dto);
        Task<bool> ClosedComplaint(string complaintNo);

	}
}
