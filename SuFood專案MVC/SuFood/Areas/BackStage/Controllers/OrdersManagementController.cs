using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    public class OrdersManagementController : Controller
    {
        private readonly SuFoodDBContext _context;

        public OrdersManagementController(SuFoodDBContext context)
        {
            _context = context;
        }

        // GET: BackStage/OrdersManagement
        public async Task<IActionResult> Index()
        {
            var suFoodDBContext = _context.Orders.Include(o => o.Account).Include(o => o.Coupon);
            return View(await suFoodDBContext.ToListAsync());
        }


		//取得所有訂單 ("/BackStage/OrdersManagement/GetAllOrders")
		[HttpGet]
		public async Task<IEnumerable<VmOrders>> GetAllOrders()
		{
            return _context.Orders.Select(x => new VmOrders
            {
                OrdersId = x.OrdersId,
                AccountId = x.AccountId,
                BuyMethod = x.BuyMethod,
                OrderStatus = x.OrderStatus,
                SubTotal = x.SubTotal,
                SubDiscount = x.SubDiscount,
				SetOrdersDatetime = x.SetOrdersDatetime

            });
        }


		//取得訂單明細 ("/BackStage/OrdersManagement/GetOrdersDetails")
		//[HttpGet]
		//public object GetOrdersDetails()
		//{
		//    //return _context.OrdersDetails.Include(x => x.Order).Include(x => x.Order.Coupon).Select(x => new VmOrdersDetails
		//    //{

		//    //});

		//}


		//取得收件資訊 ("/BackStage/OrdersManagement/GetRecipientInfo")
		[HttpGet]
		public async Task<IEnumerable<VmOrders>> GetRecipientInfo(int ordersId)
		{
			var info = await _context.Orders.Where(o => o.OrdersId == ordersId).Select(o => new VmOrders
			{
				Name = o.Name,
				Phone = o.Phone,
				ShipAddress = o.ShipAddress,
				OrdersId = o.OrdersId
			})
			.ToListAsync();

			return info;
		}


		//編輯收件資訊 ("/BackStage/OrdersManagement/EditRecipientInfo")
		[HttpPost]
		public async Task<string> EditRecipientInfo([FromBody] VmOrders model)
		{
			var editinfo = await _context.Orders.FirstOrDefaultAsync(x => x.OrdersId == model.OrdersId);

			try
			{
				editinfo.Name = model.Name;
				editinfo.Phone = model.Phone;
				editinfo.ShipAddress = model.ShipAddress;
				_context.Update(editinfo);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrdersExists(model.OrdersId))
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

		//刪除訂單 ("/BackStage/OrdersManagement/DeleteOrders")
		[HttpDelete]
		public async Task<string> DeleteOrders(int OrdersId)
		{
			var ordersdetails = _context.OrdersDetails.Where(x => x.OrderId == OrdersId).Select(x => x);
			_context.OrdersDetails.RemoveRange(ordersdetails);
			await _context.SaveChangesAsync();

			var orders = _context.Orders.Where(o => o.OrdersId == OrdersId).Select(o => o).SingleOrDefault();
			
			if (orders == null)
			{
				return "找不到此帳戶，刪除失敗";
			}
			_context.Orders.Remove(orders);
			await _context.SaveChangesAsync();
			return "刪除成功";
		}





		private bool OrdersExists(int id)
		{
			return (_context.Orders?.Any(e => e.OrdersId == id)).GetValueOrDefault();
		}


	}
}
