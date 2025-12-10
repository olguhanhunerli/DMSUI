using DMSUI.Entities.DTOs.Document;

namespace DMSUI.ViewModels.Document
{
    public class MyPendingApprovalsViewModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);

        public List<MyPendingDocumentDTO> Documents { get; set; } = new();
    }
}
