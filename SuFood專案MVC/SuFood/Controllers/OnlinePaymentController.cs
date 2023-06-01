using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SuFood.Models;
using SuFood.Models.DTO;
using SuFood.Services;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

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
		//傳送訂單
		//測試用，傳入值要修改
		[HttpPost]
		public NewebayInfo Payment([FromBody] NewebayInfo inModel)
		{
			//var orderId = _context.Orders.Where(x => x.OrdersId == OrderId).Select(d => d.OrdersId).FirstOrDefault();
			var productName = _context.OrdersDetails.Where(od => od.OrderId == inModel.OrderId).Select(x => x.ProductName);
			var totalPrice = _context.Orders.Where(x => x.OrdersId == inModel.OrderId).Select(x => x.SubTotal).First();
			int TP = totalPrice;
			List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("MerchantID", _configuration["OnlinePayment:MerchantID"]),
				new KeyValuePair<string, string>("RespondType", "String"),
				new KeyValuePair<string, string>("Version", "2.0"),
				new KeyValuePair<string, string>("TimeStamp", DateTime.Now.Ticks.ToString()),
				new KeyValuePair<string, string>("MerchantOrderNo", inModel.OrderId.ToString()),
				new KeyValuePair<string, string>("Amt", TP.ToJson()),
				new KeyValuePair<string, string>("ItemDesc", productName.ToJson()),
				//new KeyValuePair<string, string>("Credit", inModel.PayType.ToLower() == "credit" ? "1" : null),
				//new KeyValuePair<string, string>("ReturnURL", "")
			};
			string TradeInfoParam = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));

			var HashKey = _configuration["OnlinePayment:HashKey"];
			var HashIV = _configuration["OnlinePayment:HashIV"];
			var aesString = _aes.EncryptAES(Encoding.UTF8.GetBytes(TradeInfoParam), HashKey, HashIV);
			var shaString = _aes.EncryptSHA256($"HashKey={HashKey}&{aesString}&HashIV={HashIV}").ToUpper();
			var merchantID = _configuration["OnlinePayment:MerchantID"];

			return new NewebayInfo()
			{
				MerchantID = merchantID,
				TradeInfo = aesString,
				TradeSha = shaString,
				Version = "2.0",
			};
		}

		//接受回傳資料
		//[HttpPost]
		//public IActionResult 
	}
}
