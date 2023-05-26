using Microsoft.AspNetCore.Mvc;
using SuFood.Models;

namespace SuFood.Controllers
{
	public class CheckOutController : Controller
	{
		private readonly SuFoodDBContext _context;
		public CheckOutController(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult CheckOut()
		{
			return View();
		}
		public IActionResult Index()
		{
			return View();
		}
		
	}
}
