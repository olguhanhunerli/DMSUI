using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Customers
{
    public class CustomerCreateDTO
    {
        public string companyId { get; set; }
        public string customerCode { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}
