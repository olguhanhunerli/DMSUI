using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
	public class CalibrationFileController: Controller
	{
		private readonly ICalibrationFileManager _calibrationFileManager;
		private readonly ICalibrationManager _calibrationManager;
		public CalibrationFileController(ICalibrationFileManager calibrationFileManager, ICalibrationManager calibrationManager)
		{
			_calibrationFileManager = calibrationFileManager;
			_calibrationManager = calibrationManager;
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
		[HttpGet]
		public async Task<IActionResult> DownloadOriginal(int fileId)
		{
			var (bytes, contentType, fileName) =
				await _calibrationFileManager.DownloadCalibrationFilesAsync(fileId, false);

			return File(bytes, contentType, fileName);
		}

		[HttpGet]
		public async Task<IActionResult> DownloadPdf(int fileId)
		{
			var (bytes, contentType, fileName) =
				await _calibrationFileManager.DownloadCalibrationFilesAsync(fileId, true);

			return File(bytes, contentType, fileName);
		}
		[HttpPost]
		public async Task<IActionResult> DeleteFile(int fileId, int calibrationId)
		{
			var result = await _calibrationFileManager.DeleteFiles(fileId);
			if (result)
			{
				TempData["Success"] = "Kalibrasyon Dosyası Başarıyla Silindi.";
				return RedirectToAction("Detail", "Calibration", new { id = calibrationId });
			}

			TempData["Error"] = "Kalibrasyon Dosyası Silinirken Bir Hata Oluştu.";
			return RedirectToAction("Detail", "Calibration", new { id= calibrationId });
		}
	}
}
