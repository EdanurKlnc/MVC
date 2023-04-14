using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookEntityLayer.ViewModels;
using PhoneBookUI.Models;
using System.Diagnostics;

namespace PhoneBookUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhoneTypeManager _phoneTypeManager;
        private readonly IMemberPhoneManager _memberPhoneManager;

        public HomeController(ILogger<HomeController> logger, IPhoneTypeManager phoneTypeManager, IMemberPhoneManager memberPhoneManager)
        {
            _logger = logger;
            _phoneTypeManager = phoneTypeManager;
            this._memberPhoneManager = memberPhoneManager;
        }

        public IActionResult Index()
        {

            //eğer giriş yapmış ise giriş yapan kullanıcının rehberini model olarak sayfaya gönderelim
            if (HttpContext.User.Identity?.Name != null)
            {
                var userEmail = HttpContext.User.Identity?.Name;
                var data = _memberPhoneManager.GetAll(x => x.MemberId == userEmail).Data;
                return View(data);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize] //login olmadan sayfaya erişimi önler
        public IActionResult AddPhone()
        {
            try
            {
                var phoneTypes = _phoneTypeManager.GetAll().Data;
                ViewBag.PhoneTypes = _phoneTypeManager.GetAll().Data; //IsRemoved

                ViewBag.FirstPhoneTypeId = -1;
                if (phoneTypes.Count > 0)
                {
                    ViewBag.FirstPhoneTypeId = phoneTypes.FirstOrDefault()?.Id;
                }

                MemberPhoneViewModel model = new MemberPhoneViewModel()
                {
                    MemberId = HttpContext.User.Identity?.Name
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu" + ex.Message);
                ViewBag.PhoneTypes = new List<PhoneTypeViewModel>();
                return View();

            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddPhone(MemberPhoneViewModel model)
        {
            try
            {
                ViewBag.PhoneTypes = _phoneTypeManager.GetAll().Data;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                //1) aynı telefondan varmı? yoksa ekle
                var samePhone = _memberPhoneManager.GetByConditions(x => x.MemberId == model.MemberId && x.Phone == model.Phone).Data;
                if (samePhone != null)
                {
                    ModelState.AddModelError("", $"Bu telefon {samePhone.PhoneType.Name} türünde önceden eklenmiştir");
                    return View(model);

                }

                if (model.AnotherPhoneTypeName != null)
                {
                    //diğeri secip veritabanında bulunan bir tür girerse
                    var samePhoneType = _phoneTypeManager.GetByConditions(x => x.Name.ToLower() == model.AnotherPhoneTypeName.ToLower()).Data;
                    if (samePhoneType != null)
                    {
                        ModelState.AddModelError("", $"{samePhoneType.Name} zaten mevcuttur!");
                        return View(model);
                    }
                    //diğer ile kelediği türü alıp ıd'sini ekledik
                    PhoneTypeViewModel phoneType = new PhoneTypeViewModel()
                    {
                        CreatedDate = DateTime.Now,
                        Name = model.AnotherPhoneTypeName
                    };
                    var result = _phoneTypeManager.Add(phoneType).Data;
                    model.PhoneTypeId = result.Id;

                }

                //diğer seceneğin senaryosu yarın
                model.CreatedDate = DateTime.Now;
                model.IsRemoved = false;
                model.Phone = model.CountryCode + model.Phone;

                if (!_memberPhoneManager.Add(model).IsSuccess)
                {
                    ModelState.AddModelError("", "Ekleme Başarısız!");
                    return View(model);
                }
                TempData["AddPhoneSuccessMsg"] = "Yeni telefon numarası rehbere eklendi";
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Beklenmedik bir hata oluştu" + ex.Message);
                ViewBag.PhoneTypes = new List<PhoneTypeViewModel>();
                return View();
            }
        }
        [Authorize]
        public IActionResult DeletePone(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["DeleteFailedMsg"] = "Id değeri düzgün değil!";
                    return RedirectToAction("Index", "Home");
                }
                var phone = _memberPhoneManager.GetById(id).Data;
                if (phone == null)
                {
                    TempData["DeleteFailedMsg"] = $"Kayıt bulunamadığı için silme başarısız";
                    return RedirectToAction("Index", "Home");
                }
                if (!_memberPhoneManager.Delete(phone).IsSuccess)
                {
                    TempData["DeleteFailedMsg"] = $" silme başarısız";
                    return RedirectToAction("Index", "Home");
                }
                TempData["DeleteSuccessMsg"] = $"Telefon numarası rehberden silindi";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["DeleteFailedMsg"] = $"Beklenmedik bir hata oldu!{ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        //ajax için
        [HttpPost]
        public JsonResult PhoneDelete([FromBody] int id) //datayı data:...(id) ile aldığımız için FromBody kullanılır. (index.cshtml sayfasındaki ajax)
        {

            try
            {
                if (id <= 0)
                {
                    return Json(new { isSuccess = false, message = $"Id değeri düzgün değil!" });
                }
                var phone = _memberPhoneManager.GetById(id).Data;
                if (phone == null)
                {
                    return Json(new { isSuccess = false, message = $"Kayıt bulunamadığı için silme başarısız!" });
                }
                if (!_memberPhoneManager.Delete(phone).IsSuccess)
                {
                    return Json(new { isSuccess = false, message = $"silme başarısız!" });

                }
                var userEmail = HttpContext.User.Identity?.Name;
                var data = _memberPhoneManager.GetAll(x => x.MemberId == userEmail).Data;

                return Json(new { isSuccess = true, message = $"Telefon numarası rehberden silindi!", phones = data });

            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = $"beklenmedik bir hata oluştu! {ex.Message}" }); //burdan dönen json index.cshtml'deki response karşılık gelir.mesaj isimlendirmesi aynı olmalı


            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditPhone(int id)
        {
            try
            {
                ViewBag.PhoneTypes = _phoneTypeManager.GetAll().Data;
                if (id <= 0)
                {
                    ModelState.AddModelError("", "id değeri sıfırdan küçük olamaz!");
                    return View();
                }

                var phone = _memberPhoneManager.GetById(id).Data;
                if (phone == null)
                {
                    ModelState.AddModelError("", "Kayıt bulunamadı!");
                    return View();
                }

                var country = phone.Phone.Substring(0, 3);
                phone.CountryCode = country;
                phone.Phone = phone.Phone.Substring(3);
                return View(phone);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik hata" + ex.Message);
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditPhone(MemberPhoneViewModel model)
        {
            try
            {
                ViewBag.PhoneTypes = _phoneTypeManager.GetAll().Data;

                var phone = _memberPhoneManager.GetById(model.Id).Data;
                if (phone == null)
                {
                    ModelState.AddModelError("", "Kayıt bulunmadı!");
                    return View(model);
                }

                //Var olan bir telefonu mu yazmış?
                var samePhone = _memberPhoneManager.GetByConditions(x => x.Id!= model.Id &&
                x.MemberId == HttpContext.User.Identity.Name && x.Phone == (model.CountryCode + model.Phone)).Data;

                if (samePhone != null)
                {
                    ModelState.AddModelError("", $"{model.Phone} şeklindeki telefon {samePhone.FriendNameSurname} adlı kişiye aittir! Lütfen numarayı kontrol ediniz!");
                    return View(model);
                }

                phone.Phone = model.CountryCode + model.Phone;
                phone.FriendNameSurname = model.FriendNameSurname;
                phone.PhoneTypeId = model.PhoneTypeId;

                if (_memberPhoneManager.Update(phone).IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Güncelleme başarısız oldu! Tekrar deneyiniz!");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik hata" + ex.Message);
                return View();
            }
        }

    }
}