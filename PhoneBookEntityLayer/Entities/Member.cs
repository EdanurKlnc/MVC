using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookEntityLayer.Entities
{
    [Table("Members")] //Members adında tablo oluşturduk
    public class Member
    {
        [Key]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Email adresi maksimum 100 karakter olmalı")]
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

        [Required] //db deki not null kutucuğuna benzer işlevde
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public DateTime? BirthDate { get; set; }
        public byte? Gender { get; set; }

        [Required]
        public bool IsRemoved { get; set; }
        public string? ForgetPasswordToken { get; set; }

        public string? Picture { get; set; }


    }
}
