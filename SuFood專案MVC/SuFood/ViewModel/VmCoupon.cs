using System.ComponentModel.DataAnnotations;

namespace SuFood.ViewModel
{
	public class VmCoupon
	{
		public int CouponId { get; set; }
		public string CouponName { get; set; }
		public string CouponDescription { get; set; }
		public decimal? CouponMinusCost { get; set; }
		public int? MinimumPurchasingAmount { get; set; }
		public DateTime? CouponStartDate { get; set; }
		public string Couponstartdate2String { get; set; }
		public DateTime? CouponEndDate { get; set; }
		public string Couponenddate2String { get; set; }
	}
}
