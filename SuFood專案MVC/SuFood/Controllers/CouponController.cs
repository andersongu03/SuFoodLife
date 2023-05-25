using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
	public class CouponController : Controller
	{
		private readonly SuFoodDBContext _context;

		public CouponController(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Coupon()
		{
			return View();
		}

		//("/Coupon/GetCouponsForFront")
		[HttpGet]
		public async Task<IEnumerable<VmCouponForFront>> GetCouponsForFront()
		{
			return _context.Coupon.Select(x => new VmCouponForFront
			{
				CouponId = x.CouponId,
				CouponName = x.CouponName,
				CouponDescription = x.CouponDescription,
				Couponenddate2String = x.CouponEndDate.ToString(),
			});
		}
		public async Task<JsonResult> GetCouponItem()
		{
			return Json(_context.Coupon);
		}
	}
}
