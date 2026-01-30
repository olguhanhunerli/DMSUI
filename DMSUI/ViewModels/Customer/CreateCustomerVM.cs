using System.ComponentModel.DataAnnotations;

namespace DMSUI.ViewModels.Customer
{
    public class CreateCustomerVM
    {
        [Required(ErrorMessage = "Müşteri kodu zorunludur.")]
        public string customerCode { get; set; }

        [Required(ErrorMessage = "Müşteri adı zorunludur.")]
        public string name { get; set; }

        public string phone { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
        public string email { get; set; }
    }
}
