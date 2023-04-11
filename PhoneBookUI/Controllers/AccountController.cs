using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.EmailSenderBusiness;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookEntityLayer.ViewModels;
using PhoneBookUI.Models;
using System.Security.Cryptography;
using System.Text;

namespace PhoneBookUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMemberManager _memberManager;
        private readonly IEmailSender _emailSender;
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;


        public AccountController(IMemberManager memberManager, IEmailSender emailSender)
        {
            _memberManager = memberManager;
            _emailSender = emailSender;
        }
        public IActionResult Register()
        {
            //bu metod sadece sayfayı getirir.(HttpGet)
            return View();//geriye sayfa göndericek

        }
        [HttpPost] //sayfadaki submite tıkladığında yazdığı bilgileri bu metoda düşecektir
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) // gelen bilgiler class içindeki annotationslara uygun değilse 
                {
                    ModelState.AddModelError("", "Gerekli alanları lütfen doldurunuz!");
                    return View(model);
                }

                // Ekleme işlemleri 

                //1) Aynı emailden tekrar kayıt olamaz! 
                var isSameEmail = _memberManager.GetByConditions
                    (x => x.Email.ToLower() == model.Email.ToLower()).Data;

                if (isSameEmail != null)
                {
                    ModelState.AddModelError("", "Dikkat bu kullanıcı sistemde zaten mevcuttur!");
                    return View(model);
                }
                MemberViewModel member = new MemberViewModel()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    Gender = model.Gender,
                    BirthDate = model.BirthDate,
                    CreatedDate = DateTime.Now,
                    IsRemoved = false
                };
                member.PasswordHash = HashPasword(model.Password, out byte[] salt);
                member.Salt = salt;

                //paraloyu unuttuğunda
                var result = _memberManager.Add(member);
                if (result.IsSuccess)
                {
                    var email = new EmailMessage()
                    {
                        To = new string[] { member.Email },
                        Subject = $"503 telefon rehberi uygulaması ",
                        Body = $"<html lang='tr'><head></head><body>" +
                        $"Merhaba sayın{member.Name} {member.Surname}, <br/>" +
                        $"Sisteme kaydınız gerçekleşmiştir.Sisteme giriş yapabilirsiniz." +
                        $"</body></html>"
                    };
                    //sonra async'ye ceveilecek
                    _emailSender.SendEmail(email);


                    //buraya kaydınız gerçekleşti yazılacak
                    return RedirectToAction("Login", "Account", new { email = model.Email });
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Beklenmedik bir sorun oluştu!!" + ex.Message);
                return View(model); //return view(model) parametre olarak model vermemizin sebebi sayfadaki bilgilerin silinmesini engellemek.
            }

        }

        [HttpGet]
        public IActionResult Login(string? email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                LoginViewModel model = new LoginViewModel()
                {
                    Email = email
                };
                return View(model);
            }
            return View(new LoginViewModel());
        }
        //sifre hashleme
        string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }

    }
}