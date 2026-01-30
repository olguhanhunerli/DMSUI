using DMSUI.Entities.DTOs.Customers;

namespace DMSUI.ViewModels.NewFolder
{
    public class EditCustomersVM
    {
        public int id { get; set; }
        public string customerCode { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string companyName { get; set; }
        public DateTime createdAt { get; set; }
        public bool isDelete { get; set; }
        public DateTime? deleteAt { get; set; }
        public string? deletedByName { get; set; }
    }
}
