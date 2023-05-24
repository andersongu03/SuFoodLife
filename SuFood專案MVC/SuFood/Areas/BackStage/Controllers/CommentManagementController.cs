using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
	[Area("BackStage")]
	public class CommentManagementController : Controller
	{
		private readonly SuFoodDBContext _context;
		public CommentManagementController(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CommentManagement()
		{
			return View();
		}
		//取得資料功能 get:"/BackStage/CommentManagement/GetComment"
		[HttpGet]
		public async Task<IEnumerable<VmComment>> GetComment()
		{
			return _context.OrdersReview.Select(or => new VmComment
			{
				ReviewId = or.ReviewId,
				RatingStar = or.RatingStar,
				Comment = or.Comment,
				OrdersId = or.OrdersId,
			});
		}
		//get:"/BackStage/CommentManagement/GetCm"
		//public async Task<JsonResult> GetCm()
		//{
		//	return Json(_context.OrdersReview);
		//}
		//刪除功能 "/BackStage/CommentManagement/DeleteComment/${id}"
		[HttpDelete]
		public async Task<string> DeleteComment(int Id)
		{
			if(_context.OrdersReview == null)
			{
				return "刪除失敗";
			}
			var OrdersId = await _context.OrdersReview.FindAsync(Id); 
			if(OrdersId == null) 
			{
				return "刪除失敗";
			}
			_context.OrdersReview.Remove(OrdersId);
			await _context.SaveChangesAsync();
			return "刪除成功";
		}
		//   /BackStage/CommentManagement/EditComment
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
