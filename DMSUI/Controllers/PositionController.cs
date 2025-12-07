using DMSUI.Entities.DTOs.Position;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Position;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionManager _positionManager;
        private readonly ICompanyManager _companyManager;
        private readonly IUserManager _userManager;

		public PositionController(IPositionManager positionManager, ICompanyManager companyManager, IUserManager userManager)
		{
			_positionManager = positionManager;
			_companyManager = companyManager;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
        {
            var positions = await _positionManager.GetAllPositionsAsync();
            return View(positions);
        }
        public async Task<IActionResult> Create()
        {
            var result = new PositionCreateViewModel
            {
                CompanyList = (await _companyManager.GetAllCompaniesAsync())
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList(),
            };
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PositionCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var dto = new PositionCreateDTO
            {
                CompanyId = model.CompanyId,
                CreatedBy = model.CreatedBy,
                Description = model.Description,
                IsActive = model.IsActive,
                Name = model.Name
            };

            await _positionManager.AddPositionAsync(dto);
            return RedirectToAction(nameof(Index)); 
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
			var position = await _positionManager.GetPositionByIdAsync(id);

			if (position == null)
				return NotFound();

			var result = new PositionEditViewModel
			{
				Id = position.Id,
				Name = position.Name,
				CompanyId = position.CompanyId,
				Description = position.Description,
				Notes = position.Notes,
				SortOrder = position.SortOrder,
                IsActive = position.IsActive,

				CompanyList = (await _companyManager.GetAllCompaniesAsync())
					.Select(c => new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name,
						Selected = c.Id == position.CompanyId
					}).ToList()
			};

			return View(result);
		}
    }
}
