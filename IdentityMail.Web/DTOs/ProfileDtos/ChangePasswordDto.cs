
using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Mevcut şifrenizi giriniz.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni şifrenizi giriniz.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarını giriniz.")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
