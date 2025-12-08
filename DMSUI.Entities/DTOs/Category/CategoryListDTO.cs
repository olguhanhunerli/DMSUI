using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Category
{
    public class CategoryListDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Code { get; set; }

        public int? ParentId { get; set; }
        public string? ParentName { get; set; }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedByName { get; set; }

        public List<CategoryListDTO> Children { get; set; } = new();
    }
}
