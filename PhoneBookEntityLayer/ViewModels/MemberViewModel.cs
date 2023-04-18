using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookEntityLayer.ViewModels
{

    public class MemberViewModel
    {
        [Required(ErrorMessage = "Email adresi boş olamaz")]
        [StringLength(100, ErrorMessage = "Email max 100 karakter olmalı")]
        public string Email { get; set; }

        [Required] //boş geçilemez
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required] //boş geçilemez
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }

        [Required]
        public string PasswordHash { get; set; } //Sifreyi haslemek için

        [Required]
        public byte[] Salt { get; set; }

        [Required] //db deki not null kutucuğu ile aynı
        public DateTime CreatedDate { get; set; }

        public DateTime? BirthDate { get; set; }
        public byte? Gender { get; set; }

        [Required]
        public bool IsRemoved { get; set; }
        public string? ForgetPasswordToken { get; set; }
        public string? Picture { get; set; }

    }
}
