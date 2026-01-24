using DMSUI.Entities.DTOs.Instruments;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DMSUI.Controllers
{
	public class InstrumentController: Controller
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
	}
}
