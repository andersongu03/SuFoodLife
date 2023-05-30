namespace SuFood.Models.DTO
{
	//訂單ID
	//public class OnlinePayment
	//{
	//	public int OrderId { get; set; }
	//}

	//金流需要的參數
	public class NewebayInfo
	{
		public int OrderId { get; set; }
		public string MerchantID { get; set; }
		public string TradeInfo { get; set; }
		public string TradeSha { get; set; }
		public string Version { get; set; }
	}

	//TradeInfo參數
	public class OnlinePaymentModel
	{
		public string PayType { get; set; }
		public string MerchantID { get; set; }
		public string RespondType { get; set; }
		public string TimeStamp { get; set; }
		public string Version { get; set; }
		public string MerchantOrderNo { get; set; }
		public int Amt { get; set; }
		public string ItemDesc { get; set; }
		public string Credit { get; set; }
		public string? ReturnURL { get; set; }

	}

	public class OnlinePaymentReturn
	{
		public string Status { get; set; }
		public string MerchantID { get; set; }
		public string TradeInfo { get; set; }
		public string TradeSha { get; set; }
		public string Version { get; set; }
	}

}
