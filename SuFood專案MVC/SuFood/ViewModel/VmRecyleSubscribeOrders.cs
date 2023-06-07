namespace SuFood.ViewModel
{
    public class VmRecyleSubscribeOrders
    {
        public int ReSubOrdersId { get; set; }
        public string ShipStatus { get; set; }
        public DateTime ShipDate { get; set; }
        public int OrdersId { get; set; }
        public IEnumerable<VmRecyleOrderDetails> RecyleOrderDetails { get; set; }
    }
    public class RecyleOrderDetails
    {
        public int RecyleOrderDetailsId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public int ReSubOrdersId { get; set; }
    }
}
