using System.ComponentModel.DataAnnotations;

namespace DMSUI.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        public string Password { get; set; }
    }
}
