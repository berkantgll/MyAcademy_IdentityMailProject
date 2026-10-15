
using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.UserDtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        public string Email { get; set; } = string.Empty;
    }
}
