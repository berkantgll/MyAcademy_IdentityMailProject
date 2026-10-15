
using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs
{
    public class EditProfileDto
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Ad 2-50 karakter arasında olmalıdır.")]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Soyad 2-50 karakter arasında olmalıdır.")]
        public string LastName { get; set; } = string.Empty;
    }
}
