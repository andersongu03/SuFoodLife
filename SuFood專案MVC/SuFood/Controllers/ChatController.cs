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
    }
}
