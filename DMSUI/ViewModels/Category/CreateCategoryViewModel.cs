using System.ComponentModel.DataAnnotations;

namespace DMSUI.ViewModels.Category
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public string Code { get; set; }

        public int CompanyId { get; set; }  

        public int SortOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }
}
