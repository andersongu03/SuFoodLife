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
				SetOrdersDatetime = x.SetOrdersDatetime,
				Name = x.Name,
				Phone = x.Phone,
				ShipAddress = x.ShipAddress

            });
        }


		//取得訂單明細("/BackStage/OrdersManagement/GetOrdersDetails")

		[HttpGet]
		public object GetOrdersDetails()
		{
			return _context.OrdersDetails.Include(x => x.Order).Select(x => new
			{
				orderdetail = new
				{
					orderId = x.OrderId,
					ordersDetailsId = x.OrdersDetailsId,
					productName = x.ProductName,
					unitPrice = x.UnitPrice,
					quantity = x.Quantity
				},
				order = x.Order,
				//coupon = x.Order.Coupon

			}).ToList();
		}

		//[HttpPost]
  //      public object GetSingelCouponId(GetSingleOrderDetailbk model)
  //      {
  //          var coupon = _context.Coupon.Where(x=>x.CouponId == model.CouponId).Select(x=>x.CouponName ).FirstOrDefault();

		//	var orderd = _context.

            
  //      }







        //      [HttpGet]
        //      public object GetOrdersDetails(int orderId)
        //      {
        //	try
        //	{
        //		return _context.OrdersDetails.Where(od => od.OrderId == orderId).Select(od => new
        //              {
        //                  ProductName = od.ProductName,
        //                  UnitPrice = od.UnitPrice,
        //                  Quantity = od.Quantity,
        //                  CouponName = _context.Coupon.Where(c => c.CouponId == od.CouponId).FirstOrDefault().CouponName,
        //                  CouponDicount = _context.Coupon.Where(c => c.CouponId == od.CouponId).FirstOrDefault().CouponMinusCost
        //              });

        //	}
        //	catch (Exception ex)
        //	{
        //		return "查不到相對應的CouponID";
        //	}

        //}



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
		public async Task<string> DeleteOrders(int OrderId)
		{
			var ordersdetails = _context.OrdersDetails.Where(x => x.OrderId == OrderId).Select(x => x);
			_context.OrdersDetails.RemoveRange(ordersdetails);
			await _context.SaveChangesAsync();

			var orders = _context.Orders.Where(o => o.OrdersId == OrderId).Select(o => o).SingleOrDefault();
			
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
