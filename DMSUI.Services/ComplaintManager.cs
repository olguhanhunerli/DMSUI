using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Entities.DTOs.Customers;
using DMSUI.Services.Interfaces;
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

		public async Task<bool> CreateComplaint(CreateComplaintDTO complaint)
        {
            return await _complaintApiClient.CreateComplaint(complaint);
        }

		public async Task<ComplaintItemsDTO> GetComplaintById(string complaintNo)
		{
			return await _complaintApiClient.GetComplaintById(complaintNo);
		}

		public async Task<PagedResultDTO<ComplaintItemsDTO>> GetComplaintsPaging(int page, int pageSize)
        {
           return await _complaintApiClient.GetComplaintsPaging(page, pageSize);
        }

		public async Task<bool> UpdateComplaint(string complaintNo, UpdateComplaintDTO dto)
		{
			return await _complaintApiClient.UpdateComplaint(complaintNo, dto);
		}
	}
}
