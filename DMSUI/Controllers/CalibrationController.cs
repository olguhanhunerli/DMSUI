using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class CalibrationController : Controller
    {
        private readonly ICalibrationManager _calibrationManager;

		public CalibrationController(ICalibrationManager calibrationManager)
		{
			_calibrationManager = calibrationManager;
		}

		public async Task<IActionResult> Index(int page =1 , int pageSize = 10)
        {
            var entity = await _calibrationManager.GetCalibrationItemsAsync(page, pageSize);
			return View(entity);
        }
    }
}
