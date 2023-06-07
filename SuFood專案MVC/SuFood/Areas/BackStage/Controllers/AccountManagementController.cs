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
        public async Task<IEnumerable<VmAccount>> GetAllAccounts()
        {
            return _context.Account.Select(x => new VmAccount
            {
                AccountId = x.AccountId,
                Account1 = x.Account1,
                Password = x.Password,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                Phone = x.Phone,
                DefaultShipAddress = x.DefaultShipAddress,                
                CreateDatetime = x.CreateDatetime,
                LasttImeLogin = x.LasttImeLogin,
                Role = x.Role,
                IsActive = x.IsActive,
            });
        }

        //修改會員資料
        [HttpPost]
        public async Task<string> EditAccounts([FromBody] VmAccount account)
        {
                var editacc = await _context.Account.FindAsync(account.AccountId);

                try
                {
                    editacc.FirstName = account.FirstName;
                    editacc.LastName = account.LastName;
                    editacc.Phone = account.Phone;
                    _context.Update(editacc);
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
        [HttpDelete]
        public async Task<string> DeleteAccounts(int id)
        {

            if (_context.Account == null)
            {
                return "刪除失敗";
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return "找不到此帳戶，刪除失敗";
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return "刪除成功";
        }



        private bool AccountExists(int id)
        {
            return (_context.Account?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
