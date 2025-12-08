using DMSUI.Entities.DTOs.Category;
using DMSUI.Entities.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryListDTO>> GetAllCategoriesAsync();
        Task<PagedResultDTO<CategoryListDTO>> GetPagedAsync(int page, int pageSize);
        Task<CategoryDetailDTO> GetCategoryByIdAsync(int id);
        Task<List<CategoryListDTO>> SearchAsync(string keyword);
        Task<List<CategoryTreeDTO>> GetTreeAsync();
        Task<bool> CreateCategoryAsync(CreateCategoryDTO dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
