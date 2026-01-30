using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Complaints;
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
        Task<bool> CreateComplaint(CreateComplaintDTO complaint);
    }
}
