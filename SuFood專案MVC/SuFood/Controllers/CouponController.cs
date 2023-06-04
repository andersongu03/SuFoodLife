﻿using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> GetCouponsForFront()
		{
			var getAccountId = HttpContext.Session.GetString("GetAccountId");

			var CouponList = await _context.CouponUsedList
				.Where(x => x.CouponId != null && x.AccountId == Convert.ToInt32(getAccountId) && x.CouponUsedOrNot == 1)
				.Include(x => x.Coupon)
				.Select(x => x.Coupon)
				.ToListAsync();


			return Json(CouponList);
		}

		[HttpPost]
		public async Task<IActionResult> EnterCouponForFront([FromBody]GetCouponViewModel model)
		{
			var getAccountId = HttpContext.Session.GetString("GetAccountId");

			var canUseCouponId =
				 _context.Coupon.Where(x => x.CouponStartDate <= DateTime.Now && x.CouponEndDate >= DateTime.Now && x.CouponName == model.CouponName)
				.Select(x => x.CouponId).First();

			var newCoupon =  new CouponUsedList
			{
				CouponId = canUseCouponId,
				CouponUsedOrNot = 1,
				AccountId = Convert.ToInt32(getAccountId),
			};

			_context.CouponUsedList.Add(newCoupon);
			await _context.SaveChangesAsync();

			return Json(canUseCouponId);
		}

	}
}
