using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CAPA
{
    public class CAPACreateDefaultsDTO
    {
        public string? ComplaintNo { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int OwnerId { get; set; }
        public string? OwnerName { get; set; }

        public DateTime DueDate { get; set; }

        public string? Status { get; set; }
    }
}
