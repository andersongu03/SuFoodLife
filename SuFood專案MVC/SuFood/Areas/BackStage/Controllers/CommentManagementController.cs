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
		//取得資料功能 get:"/BackStage/CommentManagement/GetComment"
		[HttpGet]
		public async Task<object> GetComment()
		{
			List<VmComment> vmComments = new List<VmComment>();
			await _context.OrdersReview.Include(o => o.Orders).ForEachAsync(o => vmComments.Add(new VmComment()
			{
				ReviewId = o.ReviewId,
				RatingStar = o.RatingStar,
				Comment = o.Comment,
				OrdersId = o.OrdersId,
				AccountId =o.Orders.AccountId,
				Phone=o.Orders.Phone,
			}));
			return vmComments;
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

		[HttpPost]
		[Route("/BackStage/BackHome/CommentManagement/CreateComment")]
		public async Task<object> CreateComment([FromBody] VmComment x)
		{
			var exsist = _context.Orders.Where(o => o.OrdersId == x.OrdersId).Count();
			var Commented = _context.OrdersReview.Where(o => o.ReviewId == x.ReviewId).Count() == 1;
			if (exsist != 0 && Commented)
			{
				OrdersReview or = new OrdersReview()
				{
					ReviewId = x.ReviewId,
					RatingStar = x.RatingStar,
					Comment = x.Comment,
					OrdersId = x.OrdersId,
					Recomment = x.Recomment,
				};

				_context.OrdersReview.Update(or);
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
