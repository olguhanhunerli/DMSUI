namespace DMSUI.ViewModels.Document
{
	public class DocumentCreatePostViewModel
	{
		public int CategoryId { get; set; }
		public string DocumentCode { get; set; }

		public string? TitleTr { get; set; }
		public string? TitleEn { get; set; }

		public int DepartmentId { get; set; }

		public string? VersionNote { get; set; }
		public int RevisionNumber { get; set; }

		public List<ApprovalRowViewModel> ApprovalList { get; set; } = new();
        public List<int> AllowedDepartmentIds { get; set; } = new();
        public List<int> AllowedRoleIds { get; set; } = new();
        public List<int> AllowedUserIds { get; set; } = new();

    }
}
