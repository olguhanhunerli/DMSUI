using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Customers
{
    public class CustomersItemDTO
    {
        public int id { get; set; }
        public string customerCode { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public DateTime? createdAt { get; set; }
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string? deletedByName { get; set; }
        public DateTime? deleteAt { get; set; }
        public bool? isDelete { get; set; }
    }
}
