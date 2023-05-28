using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;
using System.Linq;

namespace SuFood.Controllers
{
	public class CommentController : Controller
	{
		private readonly SuFoodDBContext _context;
		public CommentController(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<JsonResult> Comment()
		{
			return Json(_context.OrdersReview);
		}
		//"/Comment/CreateComment"
		[HttpPost]
		public async Task<string> CreateComment([FromBody]OrdersReview Comment)
		{
			
			var exsist = _context.Orders.Where(o => o.OrdersId == Comment.OrdersId).Count();
			if (exsist != 0) { 
				_context.Add(Comment);
				await _context.SaveChangesAsync();
				return "新增成功";
			}
			return "新增失敗";

		}
		[HttpPost]
		public async Task<string> EditComment([FromBody] OrdersReview Comment)
		{
			try
			{
				_context.OrdersReview.Select(or => new OrdersReview
				{
					ReviewId = or.ReviewId,
					RatingStar=or.RatingStar,
					Comment = or.Comment,
					OrdersId=or.OrdersId,
				});
				_context.Update(Comment);
				await _context.SaveChangesAsync();
				return " 修改成功";
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderReviewExists(Comment.ReviewId))
				{
					return "修改資料庫失敗";
				}
				else
				{
					throw;
				}
			}
		}
		private bool OrderReviewExists(int id)
		{
			return (_context.OrdersReview?.Any(e => e.ReviewId == id)).GetValueOrDefault();
		}
	}
}
