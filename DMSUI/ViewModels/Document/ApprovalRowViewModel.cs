namespace DMSUI.ViewModels.Document
{
	public class ApprovalRowViewModel
	{
		public bool IsSelected { get; set; }    
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string PositionName { get; set; }
		public int ApprovalLevel { get; set; } = 1;
	}
}
