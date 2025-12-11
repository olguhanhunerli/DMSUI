using Microsoft.AspNetCore.Http;

namespace DMSUI.Controllers
{
    public class CreateDocumentDTO
    {
        public string? TitleTr { get; set; }
        public string? TitleEn { get; set; }

        public int CategoryId { get; set; }
        public int DepartmentId { get; set; }

        public string? DocumentType { get; set; }
        public string? VersionNote { get; set; }

        public int RevisionNumber { get; set; }
        public bool IsPublic { get; set; }

        public List<int> ApproverUserIds { get; set; } = new();

        public IFormFile? MainFile { get; set; }                 // ✔ ANA DOSYA
        public List<IFormFile>? Attachments { get; set; } = new(); // ✔ EK DOSYALAR
    }
}