using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;

namespace PhoneBookUI.Components
{
    public class MenuViewComponent : ViewComponent
    {
        //ViewComponentler sayfa parcalarını yönettiğimiz yapıdır.

        //Bu nedenle controllerların içinde yaptığımız DI burada da yapabiliriz

        private readonly IMemberManager _memberManager;

        public MenuViewComponent(IMemberManager memberManager)
        {
            _memberManager = memberManager;
        }
        public IViewComponentResult Invoke()
        {
            string? userEmail = HttpContext.User.Identity?.Name; //email yazılacak
            TempData["LoggedInUserNameSurname"] = null;
            if(userEmail != null)
            {
                var user = _memberManager.GetById(userEmail).Data;
                TempData["LoggedInUserNameSurname"] = $"{user.Name} {user.Surname}";
            }
            return View();
        }
    }
}
