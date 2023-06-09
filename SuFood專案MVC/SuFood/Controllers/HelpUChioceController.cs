using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SuFood.Models;
using SuFood.Services;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
	public class HelpUChioceController : Controller
	{
		private readonly SuFoodDBContext _context;

		public HelpUChioceController(SuFoodDBContext context)
		{
			this._context = context;
		}

		public IActionResult HelpUBuy()
		{
			return View();
		}

        public IActionResult HelpUBuy2()
        {
            return View();
        }

        [HttpGet]
		public async Task<IEnumerable<GetHelpProductViewModels>> GetHelpProduct()
		{
			var HelpProduct = _context.Products
				.Where(x => x.IsHelpUchioce == true)
				.Select(x => new GetHelpProductViewModels
				{
					ProductId = x.ProductId,
					ProductName = x.ProductName,
					ProductDescription = x.ProductDescription,
					Img = x.Img,
				})
				.ToList();

			return HelpProduct;
		}

		[HttpPost]
		public async Task<bool> CreateHelpOrder([FromBody] HelpOrder2BackViewModel model)
		{
			var getAccountId = HttpContext.Session.GetString("GetAccountId");

			if (getAccountId == null)
			{
				return false;
			}

			var oldOrderId = _context.Orders.FirstOrDefault(x => x.AccountId == Convert.ToInt32(getAccountId) && x.BuyMethod == "幫你選" && x.OrderStatus == "未付款")?.OrdersId;
			var oldOrder = _context.Orders.FirstOrDefault(x => x.AccountId == Convert.ToInt32(getAccountId) && x.BuyMethod == "幫你選" && x.OrderStatus == "未付款");

			if (oldOrder != null)
			{
				var oldRecyleOrderId = _context.RecyleSubscribeOrders.Where(x => x.OrdersId == oldOrderId).Select(x => x.ReSubOrdersId).ToList();

				foreach (var o in oldRecyleOrderId)
				{
					var oldRecyleOrderDetailList = _context.RecyleOrderDetails.Where(x => x.ReSubOrdersId == o).ToList();
					_context.RemoveRange(oldRecyleOrderDetailList);
					_context.SaveChanges();
				}

				var oldRecyleOrderList = _context.RecyleSubscribeOrders.Where(x => x.OrdersId == oldOrderId).ToList();
				_context.RemoveRange(oldRecyleOrderList);				
				_context.Remove(oldOrder);
				_context.SaveChanges();

			}

			var ShipTimes = (model.TimeRange * 28) / (model.OrderFrequency * 7);
			var getTotalPay = model.OrderTotalPay * ShipTimes;
			var getSubCost = getTotalPay * 0.6;

			var order = new Orders
			{
				SubTotal = getTotalPay,
				SubCost = (int)Math.Round(getSubCost),
				SubDiscount = 0,
				SetOrdersDatetime = DateTime.Now,
				OrderStatus = "未付款",
				AccountId = Convert.ToInt32(getAccountId),
				BuyMethod = "幫你選"
			};

			_context.Orders.Add(order);
			_context.SaveChanges(); 

			var getNewOrder = order.OrdersId;

			for (var i = 0; i < ShipTimes; i++)
			{
				var HelpRecyleOrder = new RecyleSubscribeOrders
				{
					OrdersId = getNewOrder,
					ShipStatus = "未出貨",
					ShipDate = model.StartOrderDate.AddDays(i * 7),
				};

				_context.RecyleSubscribeOrders.Add(HelpRecyleOrder);
				_context.SaveChanges(); 

				var SubscribeOrdersId = HelpRecyleOrder.ReSubOrdersId;

				var random = new Random();
				var productCount = model.OrderTotalPay / 50;
				var userChoices = model.UserChioce.Select(c => c.ProductName).ToList();
				var recyleOrderDetails = new List<RecyleOrderDetails>();

				for (var j = 0; j < productCount; j++)
				{
					var randomProductIndex = random.Next(userChoices.Count);
					var productName = userChoices[randomProductIndex];

					var checkSubId = recyleOrderDetails.FirstOrDefault(d => d.ReSubOrdersId == SubscribeOrdersId);
					if(checkSubId != null)
					{
						var orderDetail = recyleOrderDetails.FirstOrDefault(d => d.ProductName == productName && d.ReSubOrdersId == SubscribeOrdersId);

						if (orderDetail != null)
						{
							orderDetail.Quantity++;
						}
						else
						{
							orderDetail = new RecyleOrderDetails
							{
								ProductName = productName,
								Quantity = 1,
								ReSubOrdersId = SubscribeOrdersId
							};
							recyleOrderDetails.Add(orderDetail);
						}
					}
					else
					{
						var newOrderDetail = new RecyleOrderDetails
						{
							ProductName = productName,
							Quantity = 1,
							ReSubOrdersId = SubscribeOrdersId
						};
						recyleOrderDetails.Add(newOrderDetail);
					}

					
				}

				_context.RecyleOrderDetails.AddRange(recyleOrderDetails);
				_context.SaveChanges(); 
			}

			return true;
		}
	}
}
