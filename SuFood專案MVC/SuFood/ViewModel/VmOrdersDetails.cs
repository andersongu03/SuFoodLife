namespace SuFood.ViewModel
{
	public class VmOrdersDetails
	{
		public int? OrderId { get; set; }
		public string ProductName { get; set; }
		public int? UnitPrice { get; set; }
		public int? Quantity { get; set; }
		public int? CouponId { get; set; }
		public int OrdersDetailsId { get; set; }
	}
}
