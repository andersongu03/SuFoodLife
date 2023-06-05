using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;

namespace SuFood.Controllers
{
    public class ChatController : Controller
    {
        private readonly SuFoodDBContext _context;
        public ChatController(SuFoodDBContext context)
        {
            _context = context;
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Chat2()
        {
            return View();
        }


        //GET: "/Chat/GetAllUsersInfo" 獲取所有使用者的資訊
        [HttpGet]
        public object GetAllUsersInfo() 
        {
            var StrsenderId = HttpContext.Session.GetString("GetAccountId");
            int senderId = Convert.ToInt32(StrsenderId);
            return _context.Account.Where(a => a.AccountId != senderId).Select(a => new
            {
                Id = a.AccountId,
                Name = $"{a.FirstName}{a.LastName}",
                myId = senderId,
            });
        }

        //GET: "/Chat/GetAdminInfo" 獲取管理員的資訊
        [HttpGet]
        public object GetAdminInfo()
        {
            var StrsenderId = HttpContext.Session.GetString("GetAccountId");
            int senderId = Convert.ToInt32(StrsenderId);
            return _context.Account.Where(a => a.AccountId != senderId && a.AccountId == 22).Select(a => new
            {
                Id = a.AccountId,
                Name = $"{a.FirstName}{a.LastName}",
                myId = senderId,
            });
        }
    }
}
