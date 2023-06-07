using SuFood.Models;

namespace SuFood.ViewModel
{
    public class VmOrders
    {
        public int OrdersId { get; set; }
        public int SubTotal { get; set; }
        public int? SubCost { get; set; }
        public double? SubDiscount { get; set; }
        public DateTime? SetOrdersDatetime { get; set; }
        public string ShipAddress { get; set; }
        public string OrderStatus { get; set; }
        public int? ShippingMethodId { get; set; }
        public int? AccountId { get; set; }
        public int? CouponId { get; set; }
        public int? CustomerPaymentId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ReMark { get; set; }
        public string Email { get; set; }
        public string BuyMethod { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual ICollection<VmOrdersDetails> OrdersDetails { get; set; }

    }
}
