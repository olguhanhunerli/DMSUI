using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Category
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public int CompanyId { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
