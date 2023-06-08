using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
	[Area("BackStage")]
	public class CouponManagementController : Controller
	{
		private readonly SuFoodDBContext _context;

		public CouponManagementController(SuFoodDBContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CouponManagement()
		{
			return View();
		}

		//Get
		[HttpGet]
		public async Task<IEnumerable<VmCoupon>> GetCoupons()
		{
			return _context.Coupon.Select(c => new VmCoupon
			{
				CouponId = c.CouponId,
				CouponName = c.CouponName,
				CouponDescription = c.CouponDescription,
				CouponMinusCost = (int?)c.CouponMinusCost,
				MinimumPurchasingAmount = c.MinimumPurchasingAmount,
				Couponstartdate2String = c.CouponStartDate.ToString().Substring(0,10),
				Couponenddate2String = c.CouponEndDate.ToString().Substring(0,10),
			});
		}

		public async Task<JsonResult> GetCouponItem()
		{
			return Json(_context.Coupon);
		}

		//Create
		[HttpPost]
		//[Bind("CouponId,CouponName,CouponDescription,CouponMinusCost,MinimumPurchasingAmount,CouponStartDate,CouponEndDate")]
		public async Task<string> CreateCoupon([FromBody] VmCoupon vmCoupons)
		{
			var getAccountId = HttpContext.Session.GetString("getAccountId");
			//var getRole = HttpContext.Session.Get("getRole");

			if (vmCoupons == null)
			{
				return "新增失敗";
			}
			
			if(vmCoupons.CouponStartDate <= DateTime.Now || vmCoupons.CouponEndDate <= DateTime.Now)
			{
				return "新增失敗";
			}

			Coupon coupon = new Coupon
			{
				CouponId = vmCoupons.CouponId,
				CouponName = vmCoupons.CouponName,
				CouponDescription = vmCoupons.CouponDescription,
				CouponMinusCost = (int?)vmCoupons.CouponMinusCost,
				MinimumPurchasingAmount = vmCoupons.MinimumPurchasingAmount,
				CouponStartDate = vmCoupons.CouponStartDate,
				CouponEndDate = vmCoupons.CouponEndDate
			};

			_context.Coupon.Add(coupon);
			await _context.SaveChangesAsync();

			// 获取CouponUsedList中的最大CouponUsed_Id
			int GetCouponUsedId = await _context.CouponUsedList.MaxAsync(c => (int?)c.CouponUsedId) ?? 0;
			CouponUsedList couponUsed = new CouponUsedList
			{
				CouponUsedId = GetCouponUsedId,
				AccountId = Convert.ToInt32(getAccountId),
				CouponUsedOrNot = 1,
				CouponId = coupon.CouponId
			};

			_context.CouponUsedList.Add(couponUsed);
			await _context.SaveChangesAsync();

			return "新增成功";

		}

		//Edit
		[HttpPost]
		public async Task<string> EditCoupon([FromBody] VmCoupon vmCoupons)
		{
			if (vmCoupons == null)
			{
				return "修改失敗";
			};

			if (vmCoupons.CouponStartDate < DateTime.Now || vmCoupons.CouponEndDate < DateTime.Now)
			{
				return "修改失敗";
			}

			Coupon coupon = new Coupon
			{
				CouponId = vmCoupons.CouponId,
				CouponName = vmCoupons.CouponName,
				CouponDescription = vmCoupons.CouponDescription,
				CouponMinusCost = (int?)vmCoupons.CouponMinusCost,
				MinimumPurchasingAmount = vmCoupons.MinimumPurchasingAmount,
				CouponStartDate = vmCoupons.CouponStartDate,
				CouponEndDate = vmCoupons.CouponEndDate
			};
			_context.Update(coupon);
			await _context.SaveChangesAsync();
			return "修改成功";
		}

		//Delete
		[HttpDelete]
		public async Task<string> DeleteCoupon(int id)
		{
			var couponDel = await _context.Coupon.FindAsync(id);
			if(couponDel != null)
			{
				try
				{
					_context.Coupon.Remove(couponDel);
					await _context.SaveChangesAsync();
				}
				catch(DbUpdateConcurrencyException e)
				{
					return "找不到對應優惠券，刪除失敗";
				}

				return "刪除成功";
			}
			return "刪除失敗";
		}
		private bool CouponsExists(int id)
		{
			return (_context.Coupon?.Any(c => c.CouponId == id)).GetValueOrDefault();
		}
	}
}
