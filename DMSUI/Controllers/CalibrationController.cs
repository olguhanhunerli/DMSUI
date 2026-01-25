using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Calibration;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.CreateCalibrationVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
	public class CalibrationController : Controller
	{
		private readonly ICalibrationManager _calibrationManager;
		private readonly IInstrumentApiClient _instrumentApi;

		public CalibrationController(ICalibrationManager calibrationManager, IInstrumentApiClient instrumentApi)
		{
			_calibrationManager = calibrationManager;
			_instrumentApi = instrumentApi;
		}
		[HttpGet]
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var entity = await _calibrationManager.GetCalibrationItemsAsync(page, pageSize);
			return View(entity);
		}
		[HttpGet]
		public async Task<IActionResult> Detail(int id)
		{
			var entity = await _calibrationManager.GetCalibrationItemByIdAsync(id);
			if (entity == null)
			{
				TempData["Error"] = "Kalibrasyon kaydı bulunamadı.";
				return RedirectToAction(nameof(Index));
			}
			return View(entity);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var instruments = await _instrumentApi.GetInstrumentsAsync(1, 200);

			var vm = new CreateCalibrationVM
			{
				CalibrationDate = DateTime.Today,
				IntervalMonths = 12,
				Result = "PASS",
				Instruments = instruments.Items.Select(x => new SelectListItem
				{
					Value = x.Instrument_Id.ToString(),
					Text = x.Name
				}).ToList()
			};

			return View(vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateCalibrationVM model)
		{
			if (model.InstrumentId == 0)
				ModelState.AddModelError(nameof(model.InstrumentId), "Cihaz seçmek zorunlu.");
			var companyId = User.GetCompanyIdSafe();
			if (companyId is null)
			{
				TempData["Error"] = "Şirket bilgisi bulunamadı. Lütfen tekrar giriş yapın.";
				return RedirectToAction("Create");
			}

			if (!ModelState.IsValid)
			{
				var instruments = await _instrumentApi.GetInstrumentsAsync(1, 200);
				model.Instruments = instruments.Items.Select(x => new SelectListItem
				{
					Value = x.Instrument_Id.ToString(),
					Text = x.Name
				}).ToList();
				TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
				return View(model);
			}

			try
			{
				var dto = new CreateCalibrationDTO
				{
					InstrumentId = model.InstrumentId,
					CalibrationDate = model.CalibrationDate,
					IntervalMonths = model.IntervalMonths,
					Result = model.Result,
					CalibrationCompany = model.CalibrationCompany,
					CertificateNo = model.CertificateNo,
					CompanyId = companyId.Value,
					Location = model.Location
				};

				var id = await _calibrationManager.CreateCalibrationAsync(dto);

				if (id == null || id == 0)
				{
					TempData["Error"] = "Kalibrasyon oluşturulamadı.";
					var instruments = await _instrumentApi.GetInstrumentsAsync(1, 200);
					model.Instruments = instruments.Items.Select(x => new SelectListItem
					{
						Value = x.Instrument_Id.ToString(),
						Text = x.Name
					}).ToList();
					return View(model);
				}

				TempData["Success"] = "Kalibrasyon başarıyla oluşturuldu.";
				return RedirectToAction(nameof(Detail), new { id = (int)id.Value });
			}
			catch
			{
				TempData["Error"] = "Beklenmeyen bir hata oluştu. Lütfen tekrar deneyin.";
				var instruments = await _instrumentApi.GetInstrumentsAsync(1, 200);
				model.Instruments = instruments.Items.Select(x => new SelectListItem
				{
					Value = x.Instrument_Id.ToString(),
					Text = x.Name
				}).ToList();
				return View(model);
			}
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var entity = await _calibrationManager.GetCalibrationItemByIdAsync(id);
			if (entity == null)
			{
				TempData["Error"] = "Kalibrasyon kaydı bulunamadı.";
				return RedirectToAction(nameof(Index));
			}
			var companyId = User.GetCompanyIdSafe();
			var model = new EditCalibrationDTO
			{
				CalibrationId = (ulong)entity.CalibrationId,
				InstrumentId = (ulong)entity.InstrumentId,

				CalibrationDate = entity.CalibrationDate,
				IntervalMonths = entity.IntervalMonths,
				Result = entity.Result,
				CompanyId = companyId.Value,
				CalibrationCompany = entity.CalibrationCompany,
				CertificateNo = entity.CertificateNo,

				Location = entity.Location,

				AssetCode = entity.AssetCode,
				InstrumentName = entity.InstrumentName,
				SerialNo = entity.SerialNo,
				CompanyName = entity.CompanyName,
				DueDate = entity.DueDate,
				RemainingDays = entity.RemainingDays,
				CreatedAt = entity.CreatedAt,
				UpdatedAt = (DateTime)entity.UpdatedAt,
				CreatedByName = entity.CreatedByName,
				UpdatedByName = entity.UpdatedByName,
				Files = entity.Files
			};

			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(ulong id, EditCalibrationDTO model)
		{
			Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(model));
			if (id != model.CalibrationId)
			{
				TempData["Error"] = "Id uyuşmazlığı tespit edildi.";
				return View(model);
			}

			if (!ModelState.IsValid)
			{
				TempData["Error"] = "Lütfen zorunlu alanları kontrol edin.";
				return View(model);
			}
		
			var result = await _calibrationManager.UpdateCalibrationAsync(id, model);

			if (!result)
			{
				Console.WriteLine(result);
				TempData["Error"] = "Güncelleme başarısız.";
				return View(model);
			}

			TempData["Success"] = "Kalibrasyon başarıyla güncellendi.";
			return RedirectToAction(nameof(Detail), new { id = (int)id });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _calibrationManager.DeleteByIdAsync(id);
			if (!result)
			{
				TempData["Error"] = "Silme işlemi başarısız.";
				return RedirectToAction(nameof(Index));
			}

			TempData["Success"] = "Kalibrasyon başarıyla silindi.";
			return RedirectToAction(nameof(Index));
		}
	}
}
