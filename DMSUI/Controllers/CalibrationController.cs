using ClosedXML.Excel;
using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Calibration;
using DMSUI.Services;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.Calibration;
using DMSUI.ViewModels.Complaint;
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
					Location = model.Location,
					Notes = model.Notes
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
				CalibrationId = entity.CalibrationId,
				InstrumentId = entity.InstrumentId,

				CalibrationDate = entity.CalibrationDate,
				IntervalMonths = entity.IntervalMonths,
				Result = entity.Result,
				CompanyId = companyId.Value,
				CalibrationCompany = entity.CalibrationCompany,
				CertificateNo = entity.CertificateNo,

				InstrumentLocation = entity.InstrumentLocation,

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
				Files = entity.Files,
				Notes = entity.Notes
			};

			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditCalibrationDTO model)
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
		[HttpGet]
		public async Task<IActionResult> Pdf(int id)
		{
			var entity = await _calibrationManager.GetCalibrationItemByIdAsync(id);
			if (entity == null)
				return NotFound();

			var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
			byte[]? logoBytes = System.IO.File.Exists(logoPath) ? await System.IO.File.ReadAllBytesAsync(logoPath) : null;
			var pdfBytes = CalibrationPdfBuilder.Build(entity, logoBytes);

			return File(pdfBytes, "application/pdf", $"{id}.pdf");
		}
		[HttpGet]
		public async Task<IActionResult> ExportExcel(
	int page = 1,
	int pageSize = 1000,
	string? q = null,
	string? location = null,
	string? result = null,
	int? companyId = null
)
		{
			var res = await _calibrationManager.GetCalibrationItemsAsync(page, pageSize);
			var items = (res.Items ?? Enumerable.Empty<CalibrationItemDTO>()).AsEnumerable();

			if (!string.IsNullOrWhiteSpace(q))
			{
				var needle = q.Trim();
				items = items.Where(x =>
				{
					var hay = $"{x.InstrumentName} {x.AssetCode} {x.SerialNo} {x.CertificateNo}";
					return hay.Contains(needle, StringComparison.OrdinalIgnoreCase);
				});
			}

			if (!string.IsNullOrWhiteSpace(location))
			{
				var loc = location.Trim();
				items = items.Where(x => (x.InstrumentLocation ?? "").Contains(loc, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(result))
			{
				var r = result.Trim();
				items = items.Where(x => string.Equals((x.Result ?? "").Trim(), r, StringComparison.OrdinalIgnoreCase));
			}

			if (companyId.HasValue)
				items = items.Where(x => x.CompanyId == companyId.Value);

			var filtered = items.ToList();

			using var wb = new XLWorkbook();
			var ws = wb.Worksheets.Add("Kalibrasyonlar");

			ws.Cell(1, 1).Value = "Şirket";
			ws.Cell(1, 2).Value = "Kod";
			ws.Cell(1, 3).Value = "Cihaz";
			ws.Cell(1, 4).Value = "Seri No";
			ws.Cell(1, 5).Value = "Lokasyon";
			ws.Cell(1, 6).Value = "Kalibrasyon Tarihi";
			ws.Cell(1, 7).Value = "Geçerlilik (Due) Tarihi";
			ws.Cell(1, 8).Value = "Kalan Gün";
			ws.Cell(1, 9).Value = "Kalibrasyon Firması";
			ws.Cell(1, 10).Value = "Sertifika No";
			ws.Cell(1, 11).Value = "Sonuç";
			ws.Cell(1, 12).Value = "Dosya Yüklü Mü?";
			ws.Cell(1, 13).Value = "Oluşturan";
			ws.Cell(1, 14).Value = "Oluşturma Tarihi";
			ws.Cell(1, 15).Value = "Güncelleyen";
			ws.Cell(1, 16).Value = "Güncelleme Tarihi";
			ws.Cell(1, 17).Value = "Not";

			ws.Row(1).Style.Font.Bold = true;
			ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromHtml("#E3F2FD");
			ws.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			ws.SheetView.FreezeRows(1);

			var row = 2;
			foreach (var x in filtered)
			{
				ws.Cell(row, 1).Value = x.CompanyName ?? "";
				ws.Cell(row, 2).Value = x.AssetCode ?? "";
				ws.Cell(row, 3).Value = x.InstrumentName ?? "";
				ws.Cell(row, 4).Value = x.SerialNo ?? "";
				ws.Cell(row, 5).Value = x.InstrumentLocation ?? "";
				ws.Cell(row, 6).Value = x.CalibrationDate.ToString("dd.MM.yyyy");
				ws.Cell(row, 7).Value = x.DueDate.ToString("dd.MM.yyyy");
				ws.Cell(row, 8).Value = x.RemainingDays;
				ws.Cell(row, 9).Value = x.CalibrationCompany ?? "";
				ws.Cell(row, 10).Value = x.CertificateNo ?? "";
				ws.Cell(row, 11).Value = x.Result ?? "";
				ws.Cell(row, 12).Value = (x.Files?.Any() == true) ? "Evet" : "Hayır";
				ws.Cell(row, 13).Value = x.CreatedByName ?? "";
				ws.Cell(row, 14).Value = x.CreatedAt.ToString("dd.MM.yyyy HH:mm");
				ws.Cell(row, 15).Value = x.UpdatedByName ?? "";
				ws.Cell(row, 16).Value = x.UpdatedAt?.ToString("dd.MM.yyyy HH:mm") ?? "-";
				ws.Cell(row, 17).Value = x.Notes ?? "";
				row++;
			}

			ws.Columns().AdjustToContents();

			using var ms = new MemoryStream();
			wb.SaveAs(ms);

			return File(
				ms.ToArray(),
				"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				$"kalibrasyonlar_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
			);
		}
	}
}
