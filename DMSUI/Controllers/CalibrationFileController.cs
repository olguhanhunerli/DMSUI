using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
	public class CalibrationFileController: Controller
	{
		private readonly ICalibrationFileManager _calibrationFileManager;
		public CalibrationFileController(ICalibrationFileManager calibrationFileManager)
		{
			_calibrationFileManager = calibrationFileManager;
		}
		[HttpPost("api/CalibrationFile/upload")]
		public async Task<IActionResult> UploadCalibrationFiles([FromForm] Entities.DTOs.CalibrationFile.UploadCalibrationFileDTO dto)
		{
			var result = await _calibrationFileManager.UploadCalibrationFilesAsync(dto);
			if (result)
			{
				TempData["Success"] = "Kalibrasyon Dosyası Başarıyla Oluşturuldu.";

				return RedirectToAction("Index","Calibration");
			}
			TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
			return BadRequest();
		}
	}
}
