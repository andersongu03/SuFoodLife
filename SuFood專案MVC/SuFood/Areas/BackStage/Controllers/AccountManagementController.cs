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
            return _context.Account.Where( x => x.Role == "Customer").Select(x => new VmAccount
            {
                AccountId = x.AccountId,
                Account1 = x.Account1,
                FirstName = $"{x.FirstName}{x.LastName}",
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                Phone = x.Phone,
                DefaultShipAddress = x.DefaultShipAddress,
                CreateDatetime = x.CreateDatetime,
                LasttImeLogin = x.LasttImeLogin,
                Role = x.Role,
                IsActive = x.IsActive
            });
        }

        //修改會員資料
        [HttpPost]
        public async Task<string> EditAccounts([FromBody] VmAccount model)
        {
            var editacc = _context.Account.FirstOrDefault(x => x.AccountId == model.AccountId);
           
            try
                {
                    editacc.FirstName = model.FirstName;
                    editacc.LastName = model.LastName;
                    editacc.Phone = model.Phone;
                    _context.Update(editacc);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(model.AccountId))
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

            var account = _context.Account.Find(id);
            if (account == null)
            {
                return "找不到此帳戶，刪除失敗";
            }

			//刪除ShoppingCart關聯
			var shoppingCart = _context.ShoppingCart.Where(x => x.AccountId == id).ToList();
            if(shoppingCart.Count > 0)
            {
                _context.ShoppingCart.RemoveRange(shoppingCart);
                _context.SaveChanges();
            };

			//刪除CouponUsedList關聯
			var couponUsed = _context.CouponUsedList.Where(x => x.AccountId == id).ToList();
            if (couponUsed.Count > 0) { 
                _context.CouponUsedList.RemoveRange(couponUsed); 
                _context.SaveChanges();
            };

            //刪除Orders關聯 (只能刪零售及自由選的訂單)
            var orderId = _context.Orders.Where(x => x.AccountId == id).ToList();
            if (orderId.Count > 0)
            {
                foreach(var order in orderId)
                {
					//刪除OrdersReview關聯
					var OrderReview = _context.OrdersReview.Where(or => or.OrdersId == order.OrdersId).FirstOrDefault();
					if (OrderReview != null)
					{
						_context.OrdersReview.RemoveRange(OrderReview);
						_context.SaveChanges();
					}

					//刪除OrdersDetails關聯
					var ordersdetails = _context.OrdersDetails.Where(x => x.OrderId == order.OrdersId);
					_context.OrdersDetails.RemoveRange(ordersdetails);
					_context.SaveChanges();

					_context.Orders.Remove(order);
					_context.SaveChanges();
				}
            }

			_context.Account.Remove(account);
            _context.SaveChanges();
            return "刪除成功";
        }

        //取得訂單資料
        [HttpGet]
        public async Task<IEnumerable<VmOrders>> GetOrdersByAccountId(int accountId)
        {
            var orders =  _context.Orders
                .Where(x => x.AccountId == accountId)
                .Select(o => new VmOrders
                {
                    OrdersId = o.OrdersId,
                    SubTotal = o.SubTotal,
                    SubDiscount= o.SubDiscount,
                    SetOrdersDatetime = o.SetOrdersDatetime,
                    OrderStatus = o.OrderStatus,
                    AccountId = o.AccountId
                })
                .ToList();

            return orders;
        }




        private bool AccountExists(int id)
        {
            return (_context.Account?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
