using Microsoft.AspNetCore.Mvc;

namespace SuFood.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult PersonalInfo()
        {
            return View();
        }

        public IActionResult Coupon()
        {
            return View();
        }

        public IActionResult ModifyPassword()
        {
            return View();
        }

        public IActionResult MyOrders()
        {
            ViewBag.id = HttpContext.Session.GetString("GetAccountId");
            return View();
        }
    }
}
