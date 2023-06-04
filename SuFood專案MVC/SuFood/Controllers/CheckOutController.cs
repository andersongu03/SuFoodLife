using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.Models.DTO;
using SuFood.ViewModel;
using System.Net;
using System.Numerics;
using System.Security.Policy;
using System.Xml.Linq;
using static SuFood.ViewModel.RetailOrdersViewModel;

namespace SuFood.Controllers
{
	public class CheckOutController : Controller
	{
		private readonly SuFoodDBContext _context;
		public CheckOutController(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult CheckOut()
		{
			return View();
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult PaymentSucceed()
		{
			return View();
		}
		public IActionResult PaymentFailed()
		{
			return View();
		}

		// GET: /CheckOut/GetPlanToCart 
		[HttpGet]
		public Object GetPlanToCart()
		{
			return _context.ProductsOfPlans.GroupBy(p => p.PlanId).Select(group => new
			{
				Plan = group.Select(p => new VmPlanToCart
				{
					PlanId = p.Plan.PlanId,
					PlanName = p.Plan.PlanName,
					PlanDescription = p.Plan.PlanDescription,
					PlanPrice = p.Plan.PlanPrice,
					PlanTotalCount = p.Plan.PlanTotalCount,
					PlanCanChoiceCount = p.Plan.PlanCanChoiceCount,
					//select PlanId AS 訂單編號,
					//count(Product_Id) AS 幾筆Product
					//from ProductsOfPlans
					//group by PlanId
				}).FirstOrDefault(),
				Product = group.Select(p => new VmProductToCart
				{
					ProductId = p.Product.ProductId,
					ProductName = p.Product.ProductName,
					ProductDescription = p.Product.ProductDescription,
				})
			}); ;
		}

		//GET優惠券
		[HttpGet]
		public async Task <IEnumerable<VmCoupon>> GetCouponsToCart()
		{
			return _context.Coupon.Select(c => new VmCoupon
			{
				CouponId = c.CouponId,
				CouponName = c.CouponName,
				CouponDescription = c.CouponDescription,
				CouponMinusCost = c.CouponMinusCost,
				MinimumPurchasingAmount = c.MinimumPurchasingAmount,
				Couponstartdate2String = c.CouponStartDate.ToString().Substring(0, 10),
				Couponenddate2String = c.CouponEndDate.ToString().Substring(0, 10),
			});
		}

		//GET優惠券byID
		[HttpGet]
		public async Task<IActionResult> GetCouponsToCartById(int id)
		{
			var getCouponId = _context.Coupon.Where(c => c.CouponId == id).Select(c => new VmCoupon
			{
				MinimumPurchasingAmount = c.MinimumPurchasingAmount,
			});

			return Json(new { SelectCouponId = getCouponId });
		}

		//送出訂單(自由選)
		[HttpPost]
		public async Task<IActionResult> CreateFreeChoiceOrder([FromBody] VmSubmitOrder vmParameters)
		{
			var currentUserId = HttpContext.Session.GetString("GetAccountId");
			int UserId = Convert.ToInt32(currentUserId);

			Orders od = new Orders
			{
				OrdersId = vmParameters.OrdersId,
				Name = vmParameters.Name,
				Phone = vmParameters.Phone,
				ShipAddress = vmParameters.ShipAddress,
				SubTotal = vmParameters.SubTotal,
				SubCost = vmParameters.SubCost,
				SubDiscount = vmParameters.SubDiscount,
				SetOrdersDatetime = DateTime.Now,
				AccountId = UserId,
				OrderStatus = "未付款",
				Email = vmParameters.Email,
				ReMark = vmParameters.ReMark,
				BuyMethod = "自由選",
				OrdersDetails = vmParameters.OrdersDetails,
			};

			_context.Orders.Add(od);
			await _context.SaveChangesAsync();

			var id = od.OrdersId;
			return Json(new { OrderId = id });
		}

		//付款成功/失敗轉換頁面
		public IActionResult CheckPayment(OnlinePaymentReturn onlinePaymentReturn)
		{
			if (onlinePaymentReturn.Status == "SUCCESS")
			{
				return View("PaymentSucceed");
			}
			else
			{
				return View("PaymentFailed");
			}
		}

		[HttpGet]
		public async Task<string> GetCurrentAccountId()
		{
			return HttpContext.Session.GetString("GetAccountId");
		}
		//==============================以上阿信的==================================
		public IActionResult Retail2Order()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateRetailOrder([FromBody] RetailOrdersViewModel model)
		{
			var getAccountId = HttpContext.Session.GetString("GetAccountId");

			
			int? existingOrderId =
				_context.Orders.Where(o => o.AccountId == Convert.ToInt32(getAccountId) && o.OrderStatus == "未付款" && o.BuyMethod == "零售")
				.Select(o => o.OrdersId).FirstOrDefault();

			if (existingOrderId != 0)
			{
				int OldOrderId = (int)existingOrderId;
				return await EditRetailOrder(OldOrderId, model);
			}


			var getSubCost = 0;
			foreach (var details in model.Details)
			{
				var getSingleCost = _context.Products.Where(p => p.ProductName == details.ProductName).Select(p => p.Cost).First();
				var getCartQuantity = details.Quantity;
				getSubCost += getSingleCost * getCartQuantity;

			}


			var order = new Orders
			{
				Name = model.Order.Name,
				Phone = model.Order.Phone,
				ShipAddress = model.Order.ShipAddress,
				SubTotal = model.Order.SubTotal,
				SubCost = getSubCost,
				SubDiscount = model.Order.SubDiscount,
				SetOrdersDatetime = DateTime.Now,
				AccountId = Convert.ToInt32(getAccountId),
				//AccountId = 19,
				OrderStatus = "未付款",
				Email = model.Order.Email,
				ReMark = model.Order.ReMark,
				BuyMethod = "零售",
			};

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
			var getNewOrderId = order.OrdersId;

			foreach (var detail in model.Details)
			{
				var orderDetails = new OrdersDetails
				{
					OrderId = getNewOrderId,
					ProductName = detail.ProductName,
					UnitPrice = detail.UnitPrice,
					Quantity = detail.Quantity,
				};
				_context.OrdersDetails.Add(orderDetails);
				await _context.SaveChangesAsync();
			}

			return Json(new { GetOrderId = getNewOrderId });
		}
		[HttpPost]
		public async Task<IActionResult> EditRetailOrder(int orderId, RetailOrdersViewModel model)
		{
			var order = await _context.Orders.FindAsync(orderId);

			var getSubCost = 0;
			foreach (var details in model.Details)
			{
				var getSingleCost = _context.Products.Where(p => p.ProductName == details.ProductName).Select(p => p.Cost).First();
				var getCartQuantity = details.Quantity;
				getSubCost += getSingleCost * getCartQuantity;

			}

			if (order == null) { return Ok("我是誰我在哪"); }

			order.Name = model.Order.Name;
			order.Phone = model.Order.Phone;
			order.ShipAddress = model.Order.ShipAddress;
			order.SubCost = getSubCost;
			order.SubTotal = model.Order.SubTotal;
			order.SubDiscount = model.Order.SubDiscount;
			order.SetOrdersDatetime = DateTime.Now;
			order.ReMark = model.Order.ReMark;
			order.Email = model.Order.Email;

			await _context.SaveChangesAsync();

			var orderDetails = _context.OrdersDetails.Where(od => od.OrderId == orderId);
			_context.OrdersDetails.RemoveRange(orderDetails);

			foreach (var detail in model.Details)
			{
				var NewOrderDetails = new OrdersDetails
				{
					OrderId = orderId,
					ProductName = detail.ProductName,
					UnitPrice = detail.UnitPrice,
					Quantity = detail.Quantity,
				};
				_context.OrdersDetails.Add(NewOrderDetails);
				await _context.SaveChangesAsync();
			}

			return Json(new { GetOrderId = orderId });
		}

	}

}

