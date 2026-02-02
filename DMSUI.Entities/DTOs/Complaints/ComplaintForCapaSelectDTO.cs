using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Complaints
{
    public class ComplaintForCapaSelectDTO
    {
        public long Id { get; set; }
        public string ComplaintNo { get; set; }
        public string Title { get; set; }
        public string CustomerName { get; set; }
        public int SeverityId { get; set; }
        public DateTime ReportedAt { get; set; }
    }
}
