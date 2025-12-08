using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.Category
{
    public class CategoryTreeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategoryTreeDTO> Children { get; set; } = new();
    }
}
