namespace SuFood.ViewModel
{
    public class GetCouponViewModel
    {
        
        public int? CouponId { get; set; }
        public int? CouponUsedOrNot { get; set; }


        public string? CouponDescription { get; set; }
        public string? CouponName { get; set; }
        public decimal? CouponMinusCost { get; set; }
        public int? MinimumPurchasingAmount { get; set; }
        public DateTime? CouponStartDate { get; set; }
        public DateTime? CouponEndDate { get; set; }
    }
}
