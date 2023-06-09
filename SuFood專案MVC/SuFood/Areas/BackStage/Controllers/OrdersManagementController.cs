using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
				CouponId = x.CouponId,
				SetOrdersDatetime = x.SetOrdersDatetime,
				Name = x.Name,
				Phone = x.Phone,
				ReMark = x.ReMark,
				ShipAddress = x.ShipAddress

			});
		}


		//取得零售及自由選訂單明細("/BackStage/OrdersManagement/GetOrdersDetails")
		[HttpGet]
		public IQueryable<VmOrdersDetails> GetOrdersDetails(int orderId)
		{
			return _context.OrdersDetails
				.Where(od => od.OrderId == orderId)
				.Select(od => new VmOrdersDetails
				{
					OrderId = od.OrderId,
					ProductName = od.ProductName,
					UnitPrice = od.UnitPrice,
					Quantity = od.Quantity,
					OrdersDetailsId = od.OrdersDetailsId
				});			 
		}





		//取得幫你選訂單("/BackStage/OrdersManagement/GetRecyleOrders")
		[HttpGet]
        public IQueryable<VmRecyleSubscribeOrders> GetRecyleOrders(int orderId)
        {
			var recyleorder = _context.RecyleSubscribeOrders
				.Where(x => x.OrdersId == orderId)
				.Select(d => new VmRecyleSubscribeOrders
				{
					OrdersId = d.OrdersId,
					ReSubOrdersId = d.ReSubOrdersId,
					ShipDate = d.ShipDate,
					ShipStatus = d.ShipStatus,					
				});

            return recyleorder;
        }

		//取得幫你選訂單明細("/BackStage/OrdersManagement/GetRecyleOD")
		[HttpGet]
		public IQueryable<VmRecyleOrderDetails> GetRecyleOD(int ReSubOrdersId)
		{
			var recyleod = _context.RecyleOrderDetails
				.Where(x => x.ReSubOrdersId == ReSubOrdersId)
				.Select(od => new VmRecyleOrderDetails
				{
					RecyleOrderDetailsId = od.RecyleOrderDetailsId,
					ProductName = od.ProductName,
					Quantity = od.Quantity,
					ReSubOrdersId = od.ReSubOrdersId 

				});

			return recyleod;
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

		//刪除零售及自由選訂單 ("/BackStage/OrdersManagement/DeleteOrders")
		[HttpDelete]
		public async Task<string> DeleteOrders(int orderId)
		{
			//刪除Order Review的資料表
			var OrderReview = _context.OrdersReview.Where(or => or.OrdersId == orderId).FirstOrDefault();
			if(OrderReview != null)
			{
				_context.OrdersReview.RemoveRange(OrderReview);
				_context.SaveChanges();
			}
			

			var ordersdetails = _context.OrdersDetails.Where(x => x.OrderId == orderId);
			_context.OrdersDetails.RemoveRange(ordersdetails);
			_context.SaveChanges();

			var orders = _context.Orders.Where(o => o.OrdersId == orderId).SingleOrDefault();
			
			if (orders == null)
			{
				return "找不到此帳戶，刪除失敗";
			}
			_context.Orders.Remove(orders);
			await _context.SaveChangesAsync();
			return "刪除成功";
		}

		//刪除幫你選訂單 ("/BackStage/OrdersManagement/DeleteRecyleOrders")
		[HttpPost]

		public async Task<string> DeleteRecyleOrders(int orderId)
		{
			var recyleorderId = _context.RecyleSubscribeOrders.Where(x => x.OrdersId == orderId).Select(x => x.ReSubOrdersId).ToList();

			foreach (var o in recyleorderId) 
			{
				var recyleod = _context.RecyleOrderDetails.Where(x => x.ReSubOrdersId == o).ToList();
				_context.RecyleOrderDetails.RemoveRange(recyleod);
				await _context.SaveChangesAsync();

			}

			var recyleorder = _context.RecyleSubscribeOrders.Where(x => x.OrdersId == orderId).ToList();
			_context.RecyleSubscribeOrders.RemoveRange(recyleorder);
			await _context.SaveChangesAsync();

			var order = _context.Orders.FirstOrDefault(x => x.OrdersId == orderId);
			if (order == null) 
			{
				return "刪除失敗";
			}
			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
			return "刪除成功";
		}




		private bool OrdersExists(int id)
		{
			return (_context.Orders?.Any(e => e.OrdersId == id)).GetValueOrDefault();
		}


	}
}
