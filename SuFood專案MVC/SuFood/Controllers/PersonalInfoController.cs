using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
    public class PersonalInfoController : Controller
    {
        private readonly SuFoodDBContext _context;

        public PersonalInfoController(SuFoodDBContext context)
        {
            _context = context;
        }

        //取得會員資料 ("/PersonalInfo/GetAccount")
        [HttpGet]
        public async Task<IEnumerable<VmAccount>> GetAccount()
        {

            var acc = _context.Account.Where(user => user.AccountId == 1).Select(acc => new VmAccount
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
            });
            return acc;
        }

        //修改會員資料
        // POST: PersonalInfo/EditAccont/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public async Task<string> EditAccount([FromBody] VmAccount account)
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

       

        private bool AccountExists(int id)
        {
          return (_context.Account?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
