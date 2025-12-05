using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionManager _positionManager;

        public PositionController(IPositionManager positionManager)
        {
            _positionManager = positionManager;
        }

        public async Task<IActionResult> Index()
        {
            var positions = await _positionManager.GetAllPositionsAsync();
            return View(positions);
        }
    }
}
