using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
	
	public class CommentContrioller : Controller
	{
		private readonly SuFoodDBContext _context;
		public CommentContrioller(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult Comment()
		{
			return Json(_context.OrdersReview);
		}
		[HttpPost]
		public async Task<string> EditComment([FromBody] VmComment vmComment)
		{
			try
			{
				_context.OrdersReview.Select(or => new VmComment
				{

					Comment = or.Comment,

				});
				_context.Update(vmComment);
				await _context.SaveChangesAsync();
				return " 修改成功";
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderReviewExists(vmComment.ReviewId))
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
