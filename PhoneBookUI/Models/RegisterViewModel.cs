

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookUI.Models
{
    public class RegisterViewModel : Controller
    {
        [Required(ErrorMessage ="Ad alanı gereklidir!")]
        [StringLength(50,MinimumLength=2, ErrorMessage = "Ad maks 50 min 2 karakter olmalı!") ]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Soyad maks 50 min 5 karakter olmalı!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email alanı gereklidir!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir!")]
        
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="Şifreler aynı değil")] //Compare karşılaştırma yapar .Kimle ("Kimle karşılaştırılacak isek")
        public string ComparePassword { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        [Required]
        public byte? Gender { get; set; }
    }
}
