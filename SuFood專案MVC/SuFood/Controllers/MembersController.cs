using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.Services;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
	public class MembersController : Controller
	{
		private readonly SuFoodDBContext _context;
		private readonly EncryptService encryptService;
		public MembersController(SuFoodDBContext context, EncryptService encryptService)
		{
			this._context = context;
			this.encryptService = encryptService;
		}
		public IActionResult PersonalInfo()
		{
			return View();
		}

		public IActionResult Coupon()
		{
			return View();
		}

		public IActionResult ModifyPassword()
		{
			return View();
		}
		public IActionResult MyOrders()
		{
			return View();
		}

		[HttpPost]
		public async Task<string> MemberChangePassword (ModifyPasswordViewModel model)
		{
			var getAccountId = HttpContext.Session.GetString("GetAccountId");            

			var member = await _context.Account.FirstOrDefaultAsync(x => x.AccountId == Convert.ToInt32(getAccountId));
			var oldPassword = encryptService.Decrypt(member.Password);
			if (model.Password != oldPassword)
			{
				//ViewBag.Error = "舊密碼輸入錯誤哦"; 
				//return View("ModifyPassword");
				return "舊密碼輸入錯誤哦";
			}
			if (model.NewPassword != model.ComfirmNewPassword)
			{
				//ViewBag.Error = "新密碼輸入失敗";
				//return View("ModifyPassword");
				return "新密碼輸入失敗";
			}
			if ( oldPassword == model.NewPassword)
			{
				//ViewBag.Error = "新密碼跟舊密碼不能一樣哦";
				//return View("ModifyPassword");
				return "新密碼跟舊密碼不能一樣哦";
			}

			member.Password = encryptService.Encrypt(model.NewPassword);
			_context.Account.Update(member);
			_context.SaveChanges();
			//ViewBag.Success = "修改密碼成功";

			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Clear();
			return "修改成功";
			//return RedirectToAction("Index", "Home");
		}
	}
}
