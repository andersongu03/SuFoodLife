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
        [HttpPost]
        public async Task<IActionResult> MemberChangePassword (ModifyPasswordViewModel model)
        {
            var getAccountId = HttpContext.Session.GetString("GetAccountId");            

            var member = await _context.Account.FirstOrDefaultAsync(x => x.AccountId == Convert.ToInt32(getAccountId));
            var oldPassword = encryptService.Decrypt(member.Password);
            if (model.Password != oldPassword)
            {
                ViewBag.Error = "舊密碼輸入錯誤哦";
                return ViewBag.Error;
            }
            if (model.NewPassword != model.ComfirmNewPassword)
            {
                ViewBag.Error = "新密碼輸入失敗";
                return ViewBag.Error;
            }

            member.Password = encryptService.Encrypt(model.NewPassword);
            _context.Account.Update(member);
            _context.SaveChanges();
            ViewBag.Success = "修改密碼成功";

            return ViewBag.Success;
        }
    }
}
