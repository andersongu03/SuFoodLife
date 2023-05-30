using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    public class AccountManagementController : Controller
    {
        private readonly SuFoodDBContext _context;

        public AccountManagementController(SuFoodDBContext context)
        {
            _context = context;
        }

        public IActionResult AccountManagement() { 
            return View();
        }





        //取得會員資料 ("/BackStage/AccountManagement/GetAccount")
        [HttpGet]
        public async Task<IEnumerable<VmAccount>> GetAcc()
        {
            return _context.Account.Select(acc => new VmAccount
            {
                AccountId = acc.AccountId,
                Account1 = acc.Account1,
                Password = acc.Password,
                FirstName = acc.FirstName,
                LastName = acc.LastName,
                BirthDate = acc.BirthDate,
                Gender = acc.Gender,
                Phone = acc.Phone,
                DefaultShipAddress = acc.DefaultShipAddress,
                DefaultCreditCardNumber = acc.DefaultCreditCardNumber,
                DefaultCreditCardHolder = acc.DefaultCreditCardHolder,
                CreateDatetime = acc.CreateDatetime,
                LasttImeLogin = acc.LasttImeLogin,
                Role = acc.Role,
                IsActive = acc.IsActive,
            });
        }

        //修改會員資料
        [HttpPost]
        public async Task<string> EditAcc([FromBody] VmAccount account)
        {
            if (account.AccountId == null)
            {
                return "修改失敗";
            }

            var alertacc = await _context.Account.FindAsync(account.AccountId);
            alertacc.FirstName = account.FirstName;
            alertacc.LastName = account.LastName;
            alertacc.BirthDate = account.BirthDate;
            alertacc.Gender = account.Gender;
            alertacc.Phone = account.Phone;
            alertacc.DefaultShipAddress = account.DefaultShipAddress;
            alertacc.DefaultCreditCardNumber = account.DefaultCreditCardNumber;
            alertacc.DefaultCreditCardHolder = account.DefaultCreditCardHolder;

            try
            {
                _context.Account.Update(alertacc);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(account.AccountId))
                {
                    return "修改失敗";
                }
                else
                {
                    throw;
                }
            }
            return "修改成功";
        }

        //刪除會員資料
        [HttpPost]
        public async Task<string> DeleteAcc(int id)
        {
            if (_context.Account == null)
            {
                return "刪除失敗";
            }
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }

            await _context.SaveChangesAsync();
            return "刪除成功";
        }



        private bool AccountExists(int id)
        {
            return (_context.Account?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
