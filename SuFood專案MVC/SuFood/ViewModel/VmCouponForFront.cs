namespace SuFood.ViewModel
{
	public class VmCouponForFront
	{
		public class VmCoupon
		{
			public CouponUsedListViewModel CouponUsed { get; set; }
			public CouponViewModel Counpon { get; set; }
		}
		
		public class CouponUsedListViewModel
		{
			public int CouponUsedId { get; set; }
			public int? CouponUsedOrNot { get; set; }
			public int AccountId { get; set; }
			public int? CouponId { get; set; }
		}
		public class CouponViewModel
		{
			public int CouponId { get; set; }
			public string CouponDescription { get; set; }
			public string CouponName { get; set; }
			public decimal? CouponMinusCost { get; set; }
			public int? MinimumPurchasingAmount { get; set; }
			public DateTime? CouponStartDate { get; set; }
			public DateTime? CouponEndDate { get; set; }
		} 

		
	}
}
