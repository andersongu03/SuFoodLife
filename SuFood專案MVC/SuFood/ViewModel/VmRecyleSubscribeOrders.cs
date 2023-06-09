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
}
