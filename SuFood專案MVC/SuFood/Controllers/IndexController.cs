using Microsoft.AspNetCore.Mvc;
using SuFood.Models;

namespace SuFood.Controllers
{
	public class IndexController : Controller
	{
		private readonly SuFoodDBContext _context;

		public IndexController(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		//GET: 取得首頁資訊
		[HttpGet]
		public object GetHomePageInfo()
		{
			return _context.Announcement.Where(a => a.AnnouncementType == "首頁");
		}

		//GET: 取得輪播圖
		[HttpGet]
		public object GetCarouselInfo()
		{
			return _context.Announcement.Where(a => a.AnnouncementType == "輪播圖");
		}

		//GET: 取得優惠公告
		[HttpGet]
		public object GetAnnouncementInfo()
		{
			return _context.Announcement.Where(a => a.AnnouncementType == "優惠公告");
		}

		//GET: 取得熱賣商品
		[HttpGet]
		public object GetHotSaleInfo()
		{
			return _context.Announcement.Where(a => a.AnnouncementType == "熱賣");
		}

		[HttpGet]

		public object GetStarInfo()
		{
			return _context.OrdersReview.Where(r => r.RatingStar >= 3);
		}
	}
}
