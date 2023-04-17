using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;

namespace PhoneBookUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("a/h")]
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
        [Route("d")] //action ismi kısaltması
        public IActionResult Dashboard()
        {
            DateTime thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewBag.MontlyMemberCount = _memberManager.GetAll(x => x.CreatedDate > thisMonth.AddDays(-1)).Data.Count();

            //bu ay sisteme eklenen numara sayısı
            ViewBag.MontlyContactCount = _memberPhoneManager.GetAll(x => x.CreatedDate > thisMonth.AddDays(-1)).Data.Count();

            var members = _memberManager.GetAll().Data;

            //En son eklenen üyenin adı soyadı 
            ViewBag.LastMember = $"{members.LastOrDefault()?.Name} {members.LastOrDefault()?.Surname}";

            //Rehbere en son eklenen üyenin adı 
            var contacts = _memberPhoneManager.GetAll().Data;
            ViewBag.LastCotact = contacts.LastOrDefault()?.FriendNameSurname;

                return View();
        }
    }
}
