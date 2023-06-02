namespace SuFood.ViewModel
{    
    public class RetailOrdersViewModel
    {
        public int OrdersId { get; set; }
        public int SubTotal { get; set; }
        public int SubCost { get; set; }
        public double SubDiscount { get; set; }
        public DateTime SetOrdersDatetime { get; set; }
        public string ShipAddress { get; set; }
        public string OrderStatus { get; set; }
        public int ShippingMethodId { get; set; }
        public int AccountId { get; set; }
        public int? CouponId { get; set; }
        public int OrdersDetailsId { get; set; }
        public int CustomerPaymentId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ReMark { get; set; }
        public string Email { get; set; }

        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int? UnitPrice { get; set; }
        public int? Quantity { get; set; }
        
       
    }


}
