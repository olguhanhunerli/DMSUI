using DMSUI.Entities.DTOs.Complaints;
using DMSUI.Extensions;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DMSUI.Extensions;
using System.Threading.Tasks;
using DMSUI.ViewModels.Complaint;
using System.Text.Json;
using System.Net.WebSockets;
using ClosedXML.Excel;

namespace DMSUI.Controllers
{
	public class ComplaintController : Controller
	{
		private readonly IComplaintManager _complaintManager;
		private readonly ICustomerManager _customerManager;
		private readonly IUserManager _userManager;

		public ComplaintController(IComplaintManager complaintManager, ICustomerManager customerManager, IUserManager userManager)
		{
			_complaintManager = complaintManager;
			_customerManager = customerManager;
			_userManager = userManager;
		}
		[HttpGet]
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var entity = await _complaintManager.GetComplaintsPaging(page, pageSize);
			return View(entity);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var vm = new CreateComplaintVM();
			var companyId = User.GetCompanyIdSafe();
			if(companyId == null)
			{
				return Unauthorized();
			}
			vm.CompanyId = companyId.Value;
			var customers = await _customerManager.GetAllCustomersMini(companyId.Value);
			var users = await _userManager.GetAllUsersAsync();
			vm.Customers = customers
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				})
				.ToList();
			vm.Users = users
				.Select(u => new SelectListItem
				{
					Value = u.Id.ToString(),
					Text = u.FullName
				})
				.ToList();
			FillStaticLookups(vm);
			return View(vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateComplaintVM vm)
		{
			Console.WriteLine("Create Post HIT");
			var companyId = User.GetCompanyIdSafe();
			if (companyId is null) return Unauthorized();
			vm.CompanyId = companyId.Value;

			async Task FillLookupsAsync()
			{
				var customers = await _customerManager.GetAllCustomersMini(companyId.Value);
				vm.Customers = customers.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();

				var users = await _userManager.GetAllUsersAsync();
				vm.Users = users.Select(u => new SelectListItem(u.FullName, u.Id.ToString())).ToList();

				FillStaticLookups(vm);
			}

			if (!ModelState.IsValid)
			{
				foreach (var kv in ModelState)
				{
					foreach (var err in kv.Value.Errors)
					{
						Console.WriteLine($"ModelState ERROR - Key={kv.Key} - Error={err.ErrorMessage}");
					}
				}

				await FillLookupsAsync();
				return View(vm);
			}


			var dto = new CreateComplaintDTO
			{
				companyId = companyId.Value,
				customerId = vm.CustomerId,
				channelId = vm.ChannelId,
				typeId = vm.TypeId,
				severityId = vm.SeverityId,
				title = vm.Title,
				description = vm.Description,
				isRepeat = vm.IsRepeat,
				interimActionRequired = vm.InterimActionRequired,
				interimActionNote = vm.InterimActionNote,
				assignedTo = vm.AssignedTo,

				partNumber = vm.PartNumber,
				partRevision = vm.PartRevision,
				lotNumber = vm.LotNumber,
				serialNumber = vm.SerialNumber,
				productionDate = vm.ProductionDate,
				productionLine = vm.ProductionLine,

				customerComplaintNo = vm.CustomerComplaintNo,
				customerPO = vm.CustomerPO,
				deliveryNoteNo = vm.DeliveryNoteNo,

				quantityAffected = vm.QuantityAffected,
				containmentAction = vm.ContainmentAction
			};

			var complaintNo = await _complaintManager.CreateComplaint(dto);

			if (string.IsNullOrWhiteSpace(complaintNo))
			{
				ModelState.AddModelError("", "Create complaint failed");
				await FillLookupsAsync();
				return View(vm);
			}

			if (vm.AttachmentFile != null && vm.AttachmentFile.Length > 0)
			{
				try
				{
					var uploaded = await _complaintManager.CreateComplaintAttachment(complaintNo, vm.AttachmentFile);
					if (uploaded == null)
						TempData["Error"] = "Şikayet oluşturuldu ama dosya yüklenemedi.";
				}
				catch (Exception ex)
				{
					TempData["Error"] = $"Şikayet oluşturuldu ama dosya yüklenemedi: {ex.Message}";
				}
			}

			TempData["Success"] = "Şikayet başarıyla oluşturuldu.";
			return RedirectToAction("Details", new { complaintNo });
		}
		[HttpGet]
		public async Task<IActionResult> Details(string complaintNo)
		{
			var entity = await _complaintManager.GetComplaintById(complaintNo);
			if (entity == null)
			{
				return NotFound();
			}
			var files = await _complaintManager.GetComplaintAttachment(complaintNo);
			entity.Attachments = files;
			return View(entity);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(string complaintNo)
		{
			var item = await _complaintManager.GetComplaintById(complaintNo);
			if (item == null)
			{
				return NotFound();
			}
			var companyId = User.GetCompanyIdSafe();
			if (companyId == null)
				return Unauthorized();
			if(item.isClosed == true)
			{
				return RedirectToAction("Details", new { complaintNo = complaintNo });
			}
			var vm = new EditComplaintVM
			{
				Id = item.id,
				ComplaintNo = item.complaintNo,
				CompanyId = companyId.Value,

				CustomerId = item.customerId,
				ChannelId = item.channelId,
				TypeId = item.typeId,
				SeverityId = item.severityId,

				Title = item.title ?? "",
				Description = item.description ?? "",

				IsRepeat = item.isRepeat,
				InterimActionRequired = item.interimActionRequired == true,
				InterimActionNote = item.interimActionNote,
				AssignedTo = item.assignedTo ?? 0,

				PartNumber = item.partNumber,
				PartRevision = item.partRevision,
				LotNumber = item.lotNumber,
				SerialNumber = item.serialNumber,
				ProductionDate = item.productionDate,
				ProductionLine = item.productionLine,

				CustomerComplaintNo = item.customerComplaintNo,
				CustomerPO = item.customerPO,
				DeliveryNoteNo = item.deliveryNoteNo,

				QuantityAffected = item.quantityAffected,
				ContainmentAction = item.containmentAction,

				Status = item.status,
				IsClosed = item.isClosed
			};
			var customers = await _customerManager.GetAllCustomersMini(companyId.Value);
			vm.Customers = customers.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();

			var users = await _userManager.GetAllUsersAsync();
			vm.Users = users.Select(u => new SelectListItem(u.FullName, u.Id.ToString())).ToList();
			var files = await _complaintManager.GetComplaintAttachment(complaintNo);
			vm.Attachments = files;
			FillStaticLookups(vm);

			return View(vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditComplaintVM vm)
		{
			var companyId = User.GetCompanyIdSafe();
			if (companyId is null) return Unauthorized();
			vm.CompanyId = companyId.Value;
		
			async Task FillLookupsAsync()
			{
				var customers = await _customerManager.GetAllCustomersMini(companyId.Value);
				vm.Customers = customers.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();

				var users = await _userManager.GetAllUsersAsync();
				vm.Users = users.Select(u => new SelectListItem(u.FullName, u.Id.ToString())).ToList();

				FillStaticLookups(vm);
			}

			if (!ModelState.IsValid)
			{
				await FillLookupsAsync();
				return View(vm);
			}

			var highOrRepeat = vm.SeverityId >= 3 || vm.IsRepeat;
			if (highOrRepeat && string.IsNullOrWhiteSpace(vm.PartNumber))
				ModelState.AddModelError(nameof(vm.PartNumber), "Yüksek/Kritik veya tekrar şikayetlerde Parça No zorunludur.");
			if (highOrRepeat && string.IsNullOrWhiteSpace(vm.ContainmentAction))
				ModelState.AddModelError(nameof(vm.ContainmentAction), "Yüksek/Kritik veya tekrar şikayetlerde Containment Aksiyonu zorunludur.");

			if (!ModelState.IsValid)
			{
				await FillLookupsAsync();
				return View(vm);
			}

			var dto = new UpdateComplaintDTO
			{
				companyId = companyId.Value,
				customerId = vm.CustomerId,
				channelId = vm.ChannelId,
				typeId = vm.TypeId,
				severityId = vm.SeverityId,
				title = vm.Title,
				description = vm.Description,
				isRepeat = vm.IsRepeat,
				interimActionRequired = vm.InterimActionRequired,
				interimActionNote = vm.InterimActionNote,
				assignedTo = vm.AssignedTo,

				partNumber = vm.PartNumber,
				partRevision = vm.PartRevision,
				lotNumber = vm.LotNumber,
				serialNumber = vm.SerialNumber,
				productionDate = vm.ProductionDate,
				productionLine = vm.ProductionLine,

				customerComplaintNo = vm.CustomerComplaintNo,
				customerPO = vm.CustomerPO,
				deliveryNoteNo = vm.DeliveryNoteNo,

				quantityAffected = vm.QuantityAffected,
				containmentAction = vm.ContainmentAction
			};
			var dtoJson = JsonSerializer.Serialize(dto, new JsonSerializerOptions
			{
				WriteIndented = true
			});
			Console.WriteLine("UPDATE DTO JSON =>\n" + dtoJson);

			var complaintNo = await _complaintManager.UpdateComplaint(vm.ComplaintNo, dto);

			if (string.IsNullOrWhiteSpace(complaintNo))
			{
				ModelState.AddModelError("", "Güncelleme başarısız");
				await FillLookupsAsync();
				return View(vm);
			}

			if (vm.Attachment != null && vm.Attachment.Length > 0)
			{
				var uploaded = await _complaintManager.CreateComplaintAttachment(complaintNo, vm.Attachment);
				if (uploaded == null)
					TempData["Error"] = "Şikayet güncellendi ama dosya yüklenemedi.";
			}

			TempData["Success"] = "Şikayet başarıyla güncellendi.";
			return RedirectToAction("Details", new { complaintNo });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Close(string complaintNo)
		{
			var ok = await _complaintManager.ClosedComplaint(complaintNo);
			if (ok)
			{
				return RedirectToAction("Details", new { complaintNo = complaintNo });
			}
			ModelState.AddModelError("", "Şikayet kapatılamadı");
			var entity = await _complaintManager.GetComplaintById(complaintNo);
			if (entity == null)
			{
				return NotFound();
			}
			return View("Details", entity);
		}
		[HttpGet]
		public async Task<IActionResult> DownloadAttachment(int id)
		{
			var (fileBytes, contentType, fileName) = await _complaintManager.DownloadComplaintFilesAsync(id);

			if (fileBytes == null || fileBytes.Length == 0)
				return NotFound();

			return File(fileBytes, contentType ?? "application/octet-stream", fileName ?? "download");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteAttachment(int id, string complaintNo)
		{
			var ok = await _complaintManager.DeleteComplaintAttachment(id);
			if (!ok)
			{
				TempData["Error"] = "Dosya silme işlemi başarısız.";
			}
			else
			{
				TempData["Success"] = "Dosya başarıyla silindi.";
			}
			return RedirectToAction("Edit", new { complaintNo = complaintNo });
		}
		[HttpGet]
		public async Task<IActionResult> Pdf(string complaintNo)
		{
			var entity = await _complaintManager.GetComplaintById(complaintNo);
			if(entity == null)
				return NotFound();
			var files = await _complaintManager.GetComplaintAttachment(complaintNo);
			entity.Attachments = files;
			var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
			byte[]? logoBytes = System.IO.File.Exists(logoPath) ? await System.IO.File.ReadAllBytesAsync(logoPath) : null;
			var pdfBytes = ComplaintPdfBuilder.Build(entity, logoBytes);

			return File(pdfBytes, "application/pdf", $"{complaintNo}.pdf");
		}
		[HttpGet]
		public async Task<IActionResult> ExportExcel(
			int page = 1,
			int pageSize = 1000,

			string? q = null,
			string? status = null,
			string? isRepeat = null,
			string? needsCapa = null
		)
		{
			var result = await _complaintManager.GetComplaintsPaging(page, pageSize);

			var items = (result.Items ?? Enumerable.Empty<ComplaintItemsDTO>()).AsEnumerable();

			if (!string.IsNullOrWhiteSpace(status))
			{
				var s = status.Trim();
				items = items.Where(x => string.Equals((x.status ?? "").Trim(), s, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(isRepeat) && bool.TryParse(isRepeat, out var r))
			{
				items = items.Where(x => x.isRepeat == r);
			}
			if (!string.IsNullOrWhiteSpace(needsCapa) && bool.TryParse(needsCapa, out var c))
			{
				items = items.Where(x => x.needsCapa == c);
			}

			if (!string.IsNullOrWhiteSpace(q))
			{
				var needle = q.Trim();

				items = items.Where(x =>
				{
					var haystack =
						$"{x.complaintNo} {x.companyName} {x.customerName} {x.title} {x.assignedToName} {x.status} {x.partNumber} {x.lotNumber} {x.customerComplaintNo}";
					return haystack.Contains(needle, StringComparison.OrdinalIgnoreCase);
				});
			}

			var filtered = items.ToList();

			using var wb = new XLWorkbook();
			var ws = wb.Worksheets.Add("Şikayetler");

			ws.Cell(1, 1).Value = "Şikayet No";
			ws.Cell(1, 2).Value = "Firma";
			ws.Cell(1, 3).Value = "Müşteri";
			ws.Cell(1, 4).Value = "Başlık";
			ws.Cell(1, 5).Value = "Önem Seviyesi";
			ws.Cell(1, 6).Value = "Durum";
			ws.Cell(1, 7).Value = "Tekrar Şikayet";
			ws.Cell(1, 8).Value = "DÖF Gerekli";
			ws.Cell(1, 9).Value = "Atanan Kişi";
			ws.Cell(1, 10).Value = "Parça No";
			ws.Cell(1, 11).Value = "Lot No";
			ws.Cell(1, 12).Value = "Müşteri Şikayet No";
			ws.Cell(1, 13).Value = "Şikayet Tarihi";


			ws.Row(1).Style.Font.Bold = true;
			ws.SheetView.FreezeRows(1);

			var row = 2;
			foreach (var x in filtered)
			{
				ws.Cell(row, 1).Value = x.complaintNo ?? "";
				ws.Cell(row, 2).Value = x.companyName ?? "";
				ws.Cell(row, 3).Value = x.customerName ?? "";
				ws.Cell(row, 4).Value = x.title ?? "";
				ws.Cell(row, 5).Value = x.severityId;
				ws.Cell(row, 6).Value = x.status ?? "";
				ws.Cell(row, 7).Value = x.isRepeat ? "true" : "false";
				ws.Cell(row, 8).Value = x.needsCapa ? "true" : "false";
				ws.Cell(row, 9).Value = x.assignedToName ?? "";
				ws.Cell(row, 10).Value = x.partNumber ?? "";
				ws.Cell(row, 11).Value = x.lotNumber ?? "";
				ws.Cell(row, 12).Value = x.customerComplaintNo ?? "";
				ws.Cell(row, 13).Value = x.reportedAt.ToString("dd.MM.yyyy HH:mm");
				row++;
			}

			ws.Columns().AdjustToContents();

			using var ms = new MemoryStream();
			wb.SaveAs(ms);

			return File(
				ms.ToArray(),
				"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				$"complaints_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
			);
		}

		private void FillStaticLookups(CreateComplaintVM vm)
		{
			vm.Channels = new List<SelectListItem>
				{
					new("Telefon", "1"),
					new("Email", "2"),
					new("Web", "3"),
					new("Whatsapp", "4"),
				};

			vm.Types = new List<SelectListItem>
				{
					new("Ürün", "1"),
					new("Hizmet", "2"),
					new("Teslimat", "3"),
					new("Diğer", "4"),
				};

			vm.Severities = new List<SelectListItem>
				{
					new("Düşük", "1"),
					new("Orta", "2"),
					new("Yüksek", "3"),
					new("Kritik", "4"),
				};
		}
		private void FillStaticLookups(EditComplaintVM vm)
		{
			vm.Channels = new List<SelectListItem>
				{
					new("Telefon", "1"),
					new("Email", "2"),
					new("Web", "3"),
					new("Whatsapp", "4"),
				};

			vm.Types = new List<SelectListItem>
				{
					new("Ürün", "1"),
					new("Hizmet", "2"),
					new("Teslimat", "3"),
					new("Diğer", "4"),
				};

			vm.Severities = new List<SelectListItem>
				{
					new("Düşük", "1"),
					new("Orta", "2"),
					new("Yüksek", "3"),
					new("Kritik", "4"),
				};
		}
	}
}
