using DMSUI.Services.Interfaces;
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
    }
}
