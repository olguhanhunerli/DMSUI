using DMSUI.Entities.DTOs.Complaints;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMSUI.ViewModels.CAPA
{
    public class CAPASelectListVM
    {
        public string? SelectedComplaintNo { get; set; }
        public long? SelectedComplaintId { get; set; }

        public List<ComplaintForCapaSelectDTO> Items { get; set; } = new();
    }
}
