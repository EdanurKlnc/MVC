using PhoneBookEntityLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookEntityLayer.ViewModels
{
    public class MemberPhoneViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRemoved { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-zğüşöçıİĞÜŞÖÇ]+\s+[a-zA-zğüşöçıİĞÜŞÖÇ]*", ErrorMessage = "Ad Soyad sadece harflerden oluşmalıdır!!")]
        public string FriendNameSurname { get; set; }
        public byte PhoneTypeId { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 10)]

        [RegularExpression("^[0-9]*", ErrorMessage = "Telefon rakamlardan oluşmalıdır")]
        public string Phone { get; set; } //+905396796650
        public string MemberId { get; set; }
        public PhoneType? PhoneType { get; set; }
        public Member? Member { get; set; }
        public string? CountryCode { get; set; }

    }
}
