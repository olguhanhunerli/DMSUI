using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Category;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public CategoryManager(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<bool> CreateCategoryAsync(CreateCategoryDTO dto)
        {
            return await _categoryApiClient.CreateCategoryAsync(dto);   
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryApiClient.DeleteCategoryAsync(id);
        }

        public async Task<List<CategoryListDTO>> GetAllCategoriesAsync()
        {
            return await _categoryApiClient.GetAllCategoriesAsync();
        }

		public async Task<CategoryDetailDTO> GetCategoryByIdAsync(int id)
		{
			return await _categoryApiClient.GetCategoryByIdAsync(id);
		}

		public async Task<PagedResultDTO<CategoryListDTO>> GetPagedAsync(int page, int pageSize)
        {
           return await _categoryApiClient.GetPagedAsync(page, pageSize);
        }

        public async Task<List<CategoryTreeDTO>> GetTreeAsync()
        {
            return await _categoryApiClient.GetTreeAsync();
        }

        public async Task<List<CategoryListDTO>> SearchAsync(string keyword)
        {
            return await _categoryApiClient.SearchAsync(keyword);
        }
    }
}
