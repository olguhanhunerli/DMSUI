using Microsoft.AspNetCore.Http;

namespace DMSUI.Controllers
{
    public class CreateDocumentDTO
    {
		public string DocumentCode { get; set; }
		public string? TitleTr { get; set; }
        public string? TitleEn { get; set; }

        public int CategoryId { get; set; }
        public int DepartmentId { get; set; }

        public string? VersionNote { get; set; }

        public int RevisionNumber { get; set; }
        public bool IsPublic { get; set; }

        public List<int> ApproverUserIds { get; set; } = new();
        public List<int> AllowedDepartmentIds { get; set; } = new();
        public List<int> AllowedRoleIds { get; set; } = new();
        public List<int> AllowedUserIds { get; set; } = new();
        public IFormFile? MainFile { get; set; }                 
        public List<IFormFile>? Attachments { get; set; } = new(); 
    }
}