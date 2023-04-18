using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookUI.Areas.Admin.Models;

namespace PhoneBookUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class HomeController : Controller
    {
        private readonly IMemberManager _memberManager;
        private readonly IPhoneTypeManager _phoneTypeManager;
        private readonly IMemberPhoneManager _memberPhoneManager;

        public HomeController(IMemberManager memberManager, IPhoneTypeManager phoneTypeManager, IMemberPhoneManager memberPhoneManager)
        {
            _memberManager = memberManager;
            _phoneTypeManager = phoneTypeManager;
            _memberPhoneManager = memberPhoneManager;
        }
        [HttpGet]
        [Route("dsh")] //action ismi kısaltması
        public IActionResult Dashboard()
        {
            DateTime thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewBag.MontlyMemberCount = _memberManager.GetAll(x => x.CreatedDate > thisMonth.AddDays(-1)).Data.Count();

            //bu ay sisteme eklenen numara sayısı
            ViewBag.MontlyContactCount = _memberPhoneManager.GetAll(x => x.CreatedDate > thisMonth.AddDays(-1)).Data.Count();

            var members = _memberManager.GetAll().Data.OrderBy(x => x.CreatedDate);

            //En son eklenen üyenin adı soyadı 
            ViewBag.LastMember = $"{members.LastOrDefault()?.Name} {members.LastOrDefault()?.Surname}";

            //Rehbere en son eklenen üyenin adı 
            var contacts = _memberPhoneManager.GetAll().Data.OrderBy(x => x.CreatedDate);
            ViewBag.LastCotact = contacts.LastOrDefault()?.FriendNameSurname;

            return View();
        }

        [Route("/admin/GetPhoneTypePieData")] //buradaki admin controler'ın route'u
        public JsonResult GetPhoneTypePieData()
        {
            try
            {
                Dictionary<string, int> model = new Dictionary<string, int>();

                var data = _memberPhoneManager.GetAll().Data;
                foreach (var item in data)
                {
                    if (model.ContainsKey(item.PhoneType.Name)) // wissen kurs tipinden var mı?
                    {
                        //sayıyı 1 arttırsın
                        model[item.PhoneType.Name] += 1;
                    }
                    else
                    {
                        model.Add(item.PhoneType.Name, 1);
                    }
                } // foreach bitti

                return Json(new
                {
                    isSuccess = true,
                    message = "Veriler geldi",
                    types = model.Keys.ToArray(),
                    points = model.Values.ToArray()
                });

            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = "Veriler getirilemedi!" });

            }
        }


        //üye işlemleri.Üyeleri sitede görme
        [HttpGet]
        [Route("uye")]
        public IActionResult MemberIndex()
        {
            try
            {
                var data = _memberManager.GetAll().Data;
                return View(data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu" + ex.Message);
                return View();

            }
        }
    }
}

