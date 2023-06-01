namespace SuFood.Models.DTO
{
	//訂單ID
	public class OnlinePayment
	{
		public int OrderId { get; set; }
		public string PayType { get; set; }
	}

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

	//付款完成回傳的資料
	public class OnlinePaymentReturn
	{
		public string Status { get; set; }
		public string MerchantID { get; set; }
		public string TradeInfo { get; set; }
		public string TradeSha { get; set; }
		public string Version { get; set; }
	}

	//付款後回傳TradeInfo
	public class PaymentResult
	{
		public string Status { get; set; }
		public string Messagae { get; set;}
		public Result Result { get; set;}
	}

	// 資料回傳參數
	public class Result
	{
		public string MerchantID { get; set; }
		public string Amt { get; set; }
		public string TradeNo { get; set; }
		public string MerchantOrderNo { get; set; }
		public string PaymentType { get; set; }
		public string RespondType { get; set; }
		public DateTime PayTime { get; set; }
		public string IP { get; set; }
		public string EscrowBank { get; set; }
		public string AuthBank { get; set; }
		public string RespondCode { get; set; }
		public string Auth { get; set; }
		public string Card6No { get; set; }
		public string Card4No { get; set; }
		public int Inst { get; set; }
		public int InstFirst { get; set; }
		public int InstEach { get; set; }
		public string ECI { get; set; }
		public int TokenUseStatus { get; set; }
		public int RedAmt { get; set; }
		public string PaymentMethod { get; set; }
	}

}
