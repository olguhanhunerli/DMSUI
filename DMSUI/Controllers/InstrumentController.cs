using ClosedXML.Excel;
using DMSUI.Entities.DTOs.Instruments;
using DMSUI.Services;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
	public class InstrumentController : Controller
	{
		private readonly IInstrumentManager _manager;
		public InstrumentController(IInstrumentManager manager)
		{
			_manager = manager;
		}
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var list = await _manager.GetInstrumentsAsync(page, pageSize);
			ViewBag.Page = page;
			ViewBag.PageSize = pageSize;
			ViewBag.HasData = list.Items.Any();
			return View(list);
		}
		public async Task<IActionResult> Deleted(int page = 1, int pageSize = 10)
		{
			var list = await _manager.GetDeletedByInstrumentsAsync(page, pageSize);
			ViewBag.Page = page;
			ViewBag.PageSize = pageSize;
			ViewBag.HasData = list.Items.Any();
			return View(list);
		}
		public async Task<IActionResult> DetailDelete(int id)
		{
			var instrument = await _manager.GetInstrumentDeletedByIdAsync(id);
			if (instrument == null)
			{
				return NotFound();
			}
			return View(instrument);
		}
		public async Task<IActionResult> Detail(int id)
		{
			var instrument = await _manager.GetInstrumentByIdAsync(id);
			if (instrument == null)
			{
				return NotFound();
			}
			return View(instrument);
		}
		public async Task<IActionResult> Edit(int id)
		{
			var instrument = await _manager.GetInstrumentByIdAsync(id);
			if (instrument == null)
			{
				return NotFound();
			}
			return View(instrument);
		}
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _manager.DeleteInstrumentAsync(id);
				TempData["Success"] = "Kayıt başarıyla silindi.";
			}
			catch
			{
				TempData["Error"] = "Silme işlemi başarısız.";
			}

			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		public async Task<IActionResult> Edit(InstrumentDTO dto)
		{
			try
			{
				var updateDto = new UpdateInstrumentDTO
				{
					Name = dto.Name,
					Brand = dto.Brand,
					Model = dto.Model,
					Serial_No = dto.Serial_No,
					Measurement_Range = dto.Measurement_Range,
					Resolution = dto.Resolution,
					Unit = dto.Unit,
					Location = dto.Location,
					Owner_Person = dto.Owner_Person,
				};

				await _manager.UpdateInstrumentAsync((int)dto.Instrument_Id, updateDto);
				TempData["Success"] = "Kayıt başarıyla güncellendi.";
			}
			catch
			{
				TempData["Error"] = "Güncelleme işlemi başarısız.";
			}

			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateInstrumentDTO dto)
		{
			try
			{
				var companyIdStr = User.FindFirst("companyId")?.Value;
				if (!int.TryParse(companyIdStr, out var companyId))
					throw new Exception("CompanyId claim bulunamadı.");

				dto.CompanyId = companyId;

				await _manager.CreateInstrumentAsync(dto);
				TempData["Success"] = "Kayıt başarıyla oluşturuldu.";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				TempData["Error"] = "Kayıt oluşturulamadı.";
				return View(dto);
			}
		}
		[HttpPost]
		public async Task<IActionResult> RollBack(int id)
		{
			try
			{
				await _manager.RollBackInstrumentAsync(id);
				TempData["Success"] = "Kayıt başarıyla geri alındı.";
			}
			catch
			{
				TempData["Error"] = "Geri alma işlemi başarısız.";
			}
			return RedirectToAction(nameof(Deleted));
		}
		[HttpPost]
		public async Task<IActionResult> ToggleActive(int id, bool isActive)
		{
			try
			{
				await _manager.ToggleInstrumentActiveAsync(id, isActive);
				TempData["Success"] = isActive
					? "Kayıt aktif hale getirildi."
					: "Kayıt pasif hale getirildi.";
			}
			catch
			{
				TempData["Error"] = "Durum değiştirilemedi.";
			}

			return RedirectToAction(nameof(Index));

		}
		[HttpGet]
		public async Task<IActionResult> ExportExcel(
			int page = 1,
			int pageSize = 1000,
			string? q = null,
			string? companyId = null,
			string? location = null,
			string? instrumentType = null,
			string? riskLevel = null,
			string? isCritical = null,
				string? isActive = null
		)
		{
			var res = await _manager.GetInstrumentsAsync(page, pageSize); 
			var items = (res.Items ?? Enumerable.Empty<InstrumentDTO>()).AsEnumerable();

			if (!string.IsNullOrWhiteSpace(q))
			{
				var needle = q.Trim();
				items = items.Where(x =>
				{
					var hay = $"{x.Asset_Code} {x.Name} {x.Serial_No}";
					return hay.Contains(needle, StringComparison.OrdinalIgnoreCase);
				});
			}

			if (!string.IsNullOrWhiteSpace(companyId))
				items = items.Where(x => x.CompanyId.ToString() == companyId.Trim());

			if (!string.IsNullOrWhiteSpace(location))
				items = items.Where(x => (x.Location ?? "").Contains(location.Trim(), StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(instrumentType))
				items = items.Where(x => string.Equals((x.Instrument_Type ?? "").Trim(), instrumentType.Trim(), StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(riskLevel))
				items = items.Where(x => string.Equals((x.Risk_Level ?? "").Trim(), riskLevel.Trim(), StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(isCritical) && bool.TryParse(isCritical, out var crit))
				items = items.Where(x => x.Is_Critical == crit);

			if (!string.IsNullOrWhiteSpace(isActive) && bool.TryParse(isActive, out var act))
				items = items.Where(x => x.IsActive == act);

			var filtered = items.ToList();

			using var wb = new XLWorkbook();
			var ws = wb.Worksheets.Add("Cihazlar");

			ws.Cell(1, 1).Value = "Şirket";
			ws.Cell(1, 2).Value = "Kod";
			ws.Cell(1, 3).Value = "Cihaz";
			ws.Cell(1, 4).Value = "Tip";
			ws.Cell(1, 5).Value = "Disiplin";
			ws.Cell(1, 6).Value = "Risk";
			ws.Cell(1, 7).Value = "Kritik";
			ws.Cell(1, 8).Value = "Seri No";
			ws.Cell(1, 9).Value = "Aralık";
			ws.Cell(1, 10).Value = "Hassasiyet";
			ws.Cell(1, 11).Value = "Birim";
			ws.Cell(1, 12).Value = "Lokasyon";
			ws.Cell(1, 13).Value = "Sorumlu";
			ws.Cell(1, 14).Value = "Durum";

			ws.Row(1).Style.Font.Bold = true;
			ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromHtml("#E3F2FD");
			ws.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			ws.SheetView.FreezeRows(1);

			var row = 2;
			foreach (var x in filtered)
			{
				ws.Cell(row, 1).Value = x.CompanyName ?? "";
				ws.Cell(row, 2).Value = x.Asset_Code ?? "";
				ws.Cell(row, 3).Value = x.Name ?? "";
				ws.Cell(row, 4).Value = x.Instrument_Type ?? "";
				ws.Cell(row, 5).Value = x.Measurement_Discipline ?? "";
				ws.Cell(row, 6).Value = x.Risk_Level ?? "";
				ws.Cell(row, 7).Value = x.Is_Critical ? "Evet" : "Hayır";
				ws.Cell(row, 8).Value = x.Serial_No ?? "";
				ws.Cell(row, 9).Value = x.Measurement_Range ?? "";
				ws.Cell(row, 10).Value = x.Resolution ?? "";
				ws.Cell(row, 11).Value = x.Unit ?? "";
				ws.Cell(row, 12).Value = x.Location ?? "";
				ws.Cell(row, 13).Value = x.Owner_Person ?? "";
				ws.Cell(row, 14).Value = x.IsActive ? "Aktif" : "Pasif";
				row++;
			}

			ws.Columns().AdjustToContents();

			using var ms = new MemoryStream();
			wb.SaveAs(ms);

			return File(
				ms.ToArray(),
				"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				$"cihazlar_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
			);
		}
	}
}
