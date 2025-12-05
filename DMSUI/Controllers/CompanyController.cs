using DMSUI.Entities.DTOs.Company;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyManager _companyManager;

        public CompanyController(ICompanyManager companyManager)
        {
            _companyManager = companyManager;
        }

        public async Task<IActionResult> Index()
        {
            var company = await _companyManager.GetAllCompaniesAsync();
            return View(company);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var company = await _companyManager.GetCompanyListById(id);
            var model = new CompanyViewModel
            {
                Id = company.Id,
                CompanyCode = company.CompanyCode,
                Name = company.Name,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CompanyViewModel model)
        {
			if (!ModelState.IsValid)
				return View(model);

			var dto = new CompanyUpdateDTO
			{
				Name = model.Name,
				CompanyCode = model.CompanyCode
			};

			var result = await _companyManager.UpdateCompanyAsync(model.Id, dto);

			if (!result)
			{
				ModelState.AddModelError("", "Şirket güncellenemedi.");
				return View(model);
			}

			return RedirectToAction(nameof(Index));
		}
    }
}
