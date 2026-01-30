using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DMSUI.ViewModels
{
    public class CreateComplaintVM
    {
        [Display(Name = "Şirket")]
        [Required]
        public int CompanyId { get; set; }

        [Display(Name = "Müşteri")]
        [Required]
        public int CustomerId { get; set; }

        [Display(Name = "Kanal")]
        [Required]
        public int ChannelId { get; set; }

        [Display(Name = "Tür")]
        [Required]
        public int TypeId { get; set; }

        [Display(Name = "Önem")]
        [Required]
        public int SeverityId { get; set; }

        [Display(Name = "Başlık")]
        [Required, StringLength(200)]
        public string Title { get; set; } = "";

        [Display(Name = "Açıklama")]
        [Required]
        public string Description { get; set; } = "";

        [Display(Name = "Tekrar?")]
        public bool IsRepeat { get; set; }

        [Display(Name = "Ara Aksiyon Gerekli mi?")]
        public bool InterimActionRequired { get; set; }

        [Display(Name = "Ara Aksiyon Notu")]
        public string? InterimActionNote { get; set; }

        [Display(Name = "Atanan Kişi")]
        public int AssignedTo { get; set; }

        public List<SelectListItem> Companies { get; set; } = new();
        public List<SelectListItem> Customers { get; set; } = new();
        public List<SelectListItem> Channels { get; set; } = new();
        public List<SelectListItem> Types { get; set; } = new();
        public List<SelectListItem> Severities { get; set; } = new();
        public List<SelectListItem> Users { get; set; } = new();
    }
}
