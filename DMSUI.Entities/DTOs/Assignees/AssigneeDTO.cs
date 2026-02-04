using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Assignees
{
    public class AssigneeDTO
    {
        public int userId { get; set; }
        public bool isPrimary { get; set; }
        public string? userName { get; set; }
    }
}
