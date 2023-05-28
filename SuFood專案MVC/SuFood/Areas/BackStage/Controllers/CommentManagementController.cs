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
		//get:"/BackStage/CommentManagement/Comment"
		public async Task<JsonResult> Comment()
		{
			return Json(_context.OrdersReview);
		}
		//刪除功能 "/BackStage/CommentManagement/DeleteComment/${id}"
		[HttpDelete]
		public async Task<string> DeleteComment(int Id)
		{
			if (_context.OrdersReview == null)
			{
				return "刪除失敗";
			}
			var OrdersId = await _context.OrdersReview.FirstAsync(x => x.OrdersId == Id);
			if (OrdersId == null)
			{
				return "刪除失敗";
			}
			_context.OrdersReview.Remove(OrdersId);
			await _context.SaveChangesAsync();
			return "刪除成功";
		}

		//   /BackStage/CommentManagement/CreateComment   測試用
		[HttpPost]
		public async Task<string> CreateComment( VmComment id)
		{

			var exsist = _context.Orders.Where(o => o.OrdersId == id.OrdersId).Count();
			if (exsist != 0)
			{
				_context.OrdersReview.Add(new Models.OrdersReview()
				{
					OrdersId = id.OrdersId,
					RatingStar = id.RatingStar,
					ReviewId = id.ReviewId,
					Comment=id.Comment
				});
				await _context.SaveChangesAsync();
				return "新增成功";
			}
			return "新增失敗";

		}
		//   /BackStage/CommentManagement/EditComment
		[HttpPost]
		public async Task<string> EditComment([FromBody] OrdersReview Comment)
		{
			try
			{
				_context.OrdersReview.Select(or => new OrdersReview
				{
					ReviewId = or.ReviewId,
					RatingStar = or.RatingStar,
					Comment = or.Comment,
					OrdersId = or.OrdersId,
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
