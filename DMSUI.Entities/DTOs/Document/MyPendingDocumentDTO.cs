using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Document
{
    public class MyPendingDocumentDTO
    {
		public int Id { get; set; }
		public string DocumentCode { get; set; }
        public string Title { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; }
        public int WaitingDays { get; set; }
    }
}
