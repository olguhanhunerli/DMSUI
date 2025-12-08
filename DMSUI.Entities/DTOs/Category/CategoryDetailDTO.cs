using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Category
{
    public class CategoryDetailDTO
    {
		public int Id { get; set; }

		public string Name { get; set; }
		public string? Description { get; set; }

		public string Slug { get; set; }
		public string Code { get; set; }

		public int? ParentId { get; set; }
		public string? ParentName { get; set; }

		public int CompanyId { get; set; }
		public string? CompanyName { get; set; }

		public int SortOrder { get; set; }

		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public int CreatedBy { get; set; }
		public string? CreatedByName { get; set; }

		public int? UpdatedBy { get; set; }
		public string? UpdatedByName { get; set; }

		// ✅ Recursive Tree
		public List<CategoryTreeDTO> Children { get; set; } = new();
	}
}
