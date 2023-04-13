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
                ViewBag.PhoneTypes = _phoneTypeManager.GetAll().Data; //IsRemoved
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
                var phone = _memberPhoneManager.GetById(id).Data;
                return View(phone);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu" + ex.Message);
                return View();
            }
        }

        //güncelleme işlemi
        [HttpPost]
        [Authorize]
        public IActionResult EditPhone(MemberPhoneViewModel model)
        {
            try
            {
                var phone = _memberPhoneManager.GetById(model.Id).Data;
                phone.Phone = model.Phone;
                phone.FriendNameSurname = model.FriendNameSurname;

                _memberPhoneManager.Update(phone);
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu" + ex.Message);
                return View();
            }
        }

    }
}