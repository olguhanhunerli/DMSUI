using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Entities.DTOs.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CAPA
{
    public class CAPACreateFormDTO
    {
        public CustomerMiniDTO? Customer { get; set; }
        public ComplaintMiniDTO? Complaint { get; set; }
        public CAPACreateDefaultsDTO? Defaults { get; set; }
        public CAPACreateLookupsDTO? Lookups { get; set; }
    }
}
