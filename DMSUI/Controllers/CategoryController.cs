using DMSUI.Entities.DTOs.Category;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryManager _categoryManager;

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = await _categoryManager.GetPagedAsync(page, pageSize);

            ViewBag.Page = result.Page;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalCount = result.TotalCount;

            return View(result.Items); 
        }
        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            var result = await _categoryManager.SearchAsync(keyword);
            return View("Index", result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tree = await _categoryManager.GetTreeAsync();
            var list = new List<SelectListItem>();
            BuildSelectList(tree, list, 0);

            ViewBag.ParentList = list;

            return View(new CreateCategoryViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var tree = await _categoryManager.GetTreeAsync();
                var list = new List<SelectListItem>();
                BuildSelectList(tree, list, 0);

                ViewBag.ParentList = list;
                return View(vm);
            }

            var dto = new CreateCategoryDTO
            {
                Name = vm.Name,
                Description = vm.Description,
                ParentId = vm.ParentId,
                Code = vm.Code,
                SortOrder = vm.SortOrder,
                IsActive = vm.IsActive,
                CompanyId = vm.CompanyId,
            };

            var result = await _categoryManager.CreateCategoryAsync(dto);

            if (!result)
            {
                TempData["Error"] = "Kategori oluşturulamadı.";

                var tree = await _categoryManager.GetTreeAsync();
                var list = new List<SelectListItem>();
                BuildSelectList(tree, list, 0);

                ViewBag.ParentList = list;
                return View(vm);
            }

            TempData["Success"] = "Kategori başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]CategoryDeleteDTO dto)
        {
            var result = await _categoryManager.DeleteCategoryAsync(dto.Id);

            if (!result)
                return BadRequest("Silme başarısız");

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
            var model = new CategoryEditViewModel
            {
				Id = category.Id,
				Name = category.Name,
				Description = category.Description,
				Code = category.Code,
				ParentId = category.ParentId,
				ParentName = category.ParentName,
				CompanyId = category.CompanyId,
				CompanyName = category.CompanyName,
				SortOrder = category.SortOrder,
				IsActive = category.IsActive,
				IsDeleted = category.IsDeleted,
				CreatedAt = category.CreatedAt,
				CreatedBy = category.CreatedBy,
				CreatedByName = category.CreatedByName,
				UpdatedBy = category.UpdatedBy,
				UpdatedByName = category.UpdatedByName

			};

            var tree = await _categoryManager.GetTreeAsync();
            var list = new List<SelectListItem>();
            BuildSelectList(tree, list, 0, id);
            model.ParentSelectList = list;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel vm)
        {
            
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"❌ FIELD: {state.Key} | ERROR: {error.ErrorMessage}");
                    }
                }
                var tree = await _categoryManager.GetTreeAsync();
                var list = new List<SelectListItem>();
                BuildSelectList(tree, list, 0, vm.Id);
                vm.ParentSelectList = list;

                return View(vm);
            }

            var dto = new CategoryUpdateDTO
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                ParentId = vm.ParentId,
                SortOrder = vm.SortOrder,
                IsActive = vm.IsActive
            };

            var result = await _categoryManager.UpdateCategoryAsync(dto);

            if (!result)
            {
                TempData["Error"] = "Kategori güncellenemedi.";

                var tree = await _categoryManager.GetTreeAsync();
                var list = new List<SelectListItem>();
                BuildSelectList(tree, list, 0, vm.Id);
                vm.ParentSelectList = list;

                return View(vm);
            }

            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }
       
        private void BuildSelectList(List<CategoryTreeDTO> categoryTrees, List<SelectListItem> list, int level, int? excludeId = null)
        {
            foreach (var item in categoryTrees)
            {
                if (excludeId.HasValue && item.Id == excludeId.Value)
                    continue;

                list.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = new string('-', level * 2) + item.Name
                });

                if (item.Children != null && item.Children.Any())
                {
                    BuildSelectList(item.Children, list, level + 1, excludeId);
                }
            }
        }

    }
}
