using DMSUI.Entities.DTOs.Complaints;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DMSUI.ViewModels.Complaint
{
	public class EditComplaintVM
	{
		public long Id { get; set; }
		public string? ComplaintNo { get; set; }

		[Required]
		public int CompanyId { get; set; }

		[Display(Name = "Müşteri"), Required]
		public int CustomerId { get; set; }

		[Display(Name = "Kanal"), Required]
		public int ChannelId { get; set; }

		[Display(Name = "Tür"), Required]
		public int TypeId { get; set; }

		[Display(Name = "Önem"), Required]
		public int SeverityId { get; set; }

		[Display(Name = "Başlık"), Required, StringLength(200)]
		public string Title { get; set; } = "";

		[Display(Name = "Açıklama"), Required]
		public string Description { get; set; } = "";

		[Display(Name = "Tekrar?")]
		public bool IsRepeat { get; set; }

		[Display(Name = "Ara Aksiyon Gerekli mi?")]
		public bool InterimActionRequired { get; set; }

		[Display(Name = "Ara Aksiyon Notu")]
		public string? InterimActionNote { get; set; }

		[Display(Name = "Atanan Kişi")]
		public int AssignedTo { get; set; }

		[Display(Name = "Parça No")]
		public string? PartNumber { get; set; }

		[Display(Name = "Revizyon")]
		public string? PartRevision { get; set; }

		[Display(Name = "Lot No")]
		public string? LotNumber { get; set; }

		[Display(Name = "Seri No")]
		public string? SerialNumber { get; set; }

		[Display(Name = "Üretim Tarihi")]
		public DateTime? ProductionDate { get; set; }

		[Display(Name = "Üretim Hattı")]
		public string? ProductionLine { get; set; }

		[Display(Name = "Müşteri Şikayet No")]
		public string? CustomerComplaintNo { get; set; }

		[Display(Name = "Müşteri PO")]
		public string? CustomerPO { get; set; }

		[Display(Name = "İrsaliye / Sevkiyat No")]
		public string? DeliveryNoteNo { get; set; }

		[Display(Name = "Etkilenen Adet")]
		public int? QuantityAffected { get; set; }

		[Display(Name = "Containment Aksiyonu")]
		public string? ContainmentAction { get; set; }
		public IFormFile? Attachment { get; set; }
		public List<SelectListItem> Customers { get; set; } = new();
		public List<SelectListItem> Channels { get; set; } = new();
		public List<SelectListItem> Types { get; set; } = new();
		public List<SelectListItem> Severities { get; set; } = new();
		public List<SelectListItem> Users { get; set; } = new();
		public List<ComplaintAttachmentMiniDTO> Attachments { get; set; } = new();

		public string? Status { get; set; }
		public bool? IsClosed { get; set; }
	}
}
