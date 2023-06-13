using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SuFood.Models;
using SuFood.Services;
using SuFood.ViewModel;
using System.Net.Mail;
using System.Net;
using System.Text;
using Hangfire.Server;

namespace SuFood.Controllers
{
	public class HelpUChioceController : Controller
	{
		private readonly SuFoodDBContext _context;


		public HelpUChioceController(SuFoodDBContext context)
		{
			this._context = context;
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
					if (checkSubId != null)
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



        public void CheckSendEmail()
        {
            //RecurringJob.AddOrUpdate("myrecurringjob", () => SendEmails(), Cron.Daily(18, 30));
            RecurringJob.AddOrUpdate("myrecurringjob", () => SendEmails(), Cron.Daily(12, 00));

        }
        public void SendEmails()
        {
            var paidOrders = _context.Orders.Where(x => x.BuyMethod == "幫你選" && x.OrderStatus == "已付款").Select(x => x.OrdersId).ToList();

            foreach (var check in paidOrders)
            {
                var orders = _context.RecyleSubscribeOrders.Where(x => x.ShipDate.Date == DateTime.Now.Date && x.OrdersId == check).ToList();

                var recyleOrder = _context.RecyleSubscribeOrders.Where(x => x.ShipDate.Date == DateTime.Now.Date && x.OrdersId == check).FirstOrDefault();


                foreach (var order in orders)
                {
                    var o = _context.Orders.Where(x => x.OrdersId == order.OrdersId).FirstOrDefault();
                    var recyleOrderId = _context.RecyleSubscribeOrders.Where(x => x.OrdersId == o.OrdersId).Select(x => x.ReSubOrdersId).FirstOrDefault();
                    var countShipTimes = _context.RecyleSubscribeOrders.Count(x => x.OrdersId == o.OrdersId);


                    recyleOrder.ShipStatus = "配送中";
                    _context.Update(recyleOrder);
                    _context.SaveChanges();

                    var mail = new MailMessage()
                    {
                        From = new MailAddress("SuFood2u@gmail.com"), //寄信的信箱
                        Subject = "感謝訂購", //主旨
                        Body = $"{o.Name}您好，您的訂單編號{o.OrdersId}，於今日{DateTime.Today}成功寄送，請期待我們隨機幫您搭配的{o.SubTotal / 50 / countShipTimes}樣商品，SuFoodLife蔬服人生感謝您~",
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                    };
                    mail.To.Add(new MailAddress(o.Email)); //寄給哪位   
                    try
                    {
                        using (var sm = new SmtpClient("smtp.gmail.com", 587)) //465 ssl
                        {
                            sm.EnableSsl = true;
                            sm.Credentials = new NetworkCredential("SuFood2u@gmail.com", "okjzbnxgsmkmfwlq");
                            sm.Send(mail);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }


        }

    }
}
