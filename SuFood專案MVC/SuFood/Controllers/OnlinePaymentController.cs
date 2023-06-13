using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using SuFood.Models;
using SuFood.Models.DTO;
using SuFood.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;

namespace SuFood.Controllers
{
	public class OnlinePaymentController : Controller
	{
		private readonly SuFoodDBContext _context;
		private readonly IConfiguration _configuration;
		public AesService _aes = new AesService();
		public OnlinePaymentController(SuFoodDBContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}
		
		//傳送訂單付款
		//測試用，傳入值要修改
		[HttpPost]
		public NewebayInfo Payment([FromBody] NewebayInfo inModel)
		{
			//於資料庫撈產品資訊 + 訂單價格
			//var orderId = _context.Orders.Where(x => x.OrdersId == OrderId).Select(d => d.OrdersId).FirstOrDefault();
			var productName = _context.OrdersDetails.Where(od => od.OrderId == inModel.OrderId).Select(x => x.ProductName);
			var totalPrice = _context.Orders.Where(x => x.OrdersId == inModel.OrderId).Select(x => x.SubTotal).FirstOrDefault();
			int? TP = totalPrice;

			//打包TradeInfo需要之參數
			List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("MerchantID", _configuration["OnlinePayment:MerchantID"]),
				new KeyValuePair<string, string>("RespondType", "JSON"),
				new KeyValuePair<string, string>("Version", "2.0"),
				new KeyValuePair<string, string>("TimeStamp", DateTime.Now.Ticks.ToString()),
				new KeyValuePair<string, string>("MerchantOrderNo", inModel.OrderId.ToString()),
				new KeyValuePair<string, string>("Amt", TP.ToJson()),
				new KeyValuePair<string, string>("ItemDesc", productName.ToJson()),
				//new KeyValuePair<string, string>("Credit", inModel.PayType.ToLower() == "credit" ? "1" : null),
				new KeyValuePair<string, string>("ReturnURL", "https://37e2-2407-4d00-1c01-7e46-e8ca-2878-c98a-149f.ngrok-free.app/OnlinePayment/GetPaymentReturn")
			};
			string TradeInfoParam = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));

			//加密(打包的參數 + HashKey + HashIV)
			var hashKey = _configuration["OnlinePayment:HashKey"];
			var hashIV = _configuration["OnlinePayment:HashIV"];
			var aesString = _aes.EncryptAES(Encoding.UTF8.GetBytes(TradeInfoParam), hashKey, hashIV);
			var shaString = _aes.EncryptSHA256($"HashKey={hashKey}&{aesString}&HashIV={hashIV}").ToUpper();
			var merchantID = _configuration["OnlinePayment:MerchantID"];

			return new NewebayInfo()
			{
				MerchantID = merchantID,
				TradeInfo = aesString,
				TradeSha = shaString,
				Version = "2.0",
			};
		}

		//付款後接受回傳資料
		[HttpPost]
		public async Task<IActionResult> GetPaymentReturn([FromForm]OnlinePaymentReturn returnData)
		{
			string hashKey = _configuration["OnlinePayment:HashKey"];
			string hashIV = _configuration["OnlinePayment:HashIV"];

			string r_Status = returnData.Status;
			string r_MerchantID = returnData.MerchantID;
			string r_TradeInfo = returnData.TradeInfo;
			string r_TradeSha = returnData.TradeSha;
			string r_Version = returnData.Version;

			string decryptTradeInfo = _aes.DecryptAESHex(r_TradeInfo, hashKey, hashIV);
			PaymentResult result = JsonConvert.DeserializeObject<PaymentResult>(decryptTradeInfo);

			//// 解码URL编码的字符串
			//string decodedString = HttpUtility.UrlDecode(decryptTradeInfo);
			//// 按照键值对的形式拆分字符串
			//string[] keyValuePairs = decodedString.Split('&');
			//// 构建一个字典对象来存储键值对
			//var data = new Dictionary<string, string>();
			//foreach (string keyValuePair in keyValuePairs)
			//{
			//	string[] parts = keyValuePair.Split('=');
			//	if (parts.Length == 2)
			//	{
			//		string key = parts[0];
			//		string value = parts[1];
			//		data[key] = value;
			//	}
			//}
			//// 将字典对象转换为JSON字符串
			//string json = JsonConvert.SerializeObject(data);
			//JObject.Parse(decryptTradeInfo);
			//JArray.Parse(decryptTradeInfo);

			//PaymentResult result = JsonConvert.DeserializeObject<PaymentResult>(json);

			var orderId = Convert.ToInt32(result.Result.MerchantOrderNo);
			var orderTotal = result.Result.Amt;
			var paymentType = result.Result.PaymentType;

			if(r_Status == "SUCCESS")
			{
				var order = _context.Orders.Where(o => o.OrdersId == orderId).FirstOrDefault();
				

				if (order != null)
				{
					order.OrderStatus = "已付款";
					
					//把使用過的優惠券刪掉
					var removeUserCoupon = _context.CouponUsedList.Where(x => x.CouponId == order.CouponId && x.AccountId == order.AccountId).SingleOrDefault();
					if(removeUserCoupon != null)
					{
						removeUserCoupon.CouponUsedOrNot = 0;
						_context.CouponUsedList.Update(removeUserCoupon);
						await _context.SaveChangesAsync();
					}
					//付款成功把產品數量減掉
					var saleProductsList = _context.OrdersDetails.Where(o => o.OrderId == orderId).Select(x => new { ProductName = x.ProductName, Quantity = x.Quantity }).ToList();
					foreach (var saleSingleProduct in saleProductsList)
					{
						var productToUpdate = _context.Products.FirstOrDefault(x => x.ProductName == saleSingleProduct.ProductName);
						if (productToUpdate != null)
						{
							productToUpdate.StockQuantity -= saleSingleProduct.Quantity.Value;
							_context.SaveChanges();
						}
					}

					//寄信
					var sendEmail = order.Email;

					var mail = new MailMessage()
					{
						From = new MailAddress("SuFood2u@gmail.com"), //寄信的信箱
						Subject = "感謝訂購", //主旨
						Body = ($@"{order.Name}您好，您成功訂購SuFoodLife蔬服人生，訂單編號{order.OrdersId}，金額 {order.SubTotal - order.SubDiscount}"),
						IsBodyHtml = true,
						BodyEncoding = Encoding.UTF8,
					};
					mail.To.Add(new MailAddress(order.Email)); //寄給哪位
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
				_context.SaveChanges();
			}

			OnlinePaymentReturn onlinePaymentReturn = new OnlinePaymentReturn
			{
				MerchantID = r_MerchantID,
				Status = r_Status,
				TradeInfo = r_TradeInfo,
				TradeSha = r_TradeSha,
				Version = r_Version,
			};

			return RedirectToAction("PaymentSucceed", "CheckOut");
			
		}

		
	}
}
