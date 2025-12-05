using DMSUI.Business.Interfaces;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentManager _departmentManager;

        public DepartmentController(IDepartmentManager departmentManager)
        {
            _departmentManager = departmentManager;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentManager.GetAllDepartmentsAsync();
            return View(departments);
        }
    }
}
