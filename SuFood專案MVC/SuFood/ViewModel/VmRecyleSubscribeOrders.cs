namespace SuFood.ViewModel
{
    public class VmRecyleSubscribeOrders
    {
        public int ReSubOrdersId { get; set; }
        public string ShipStatus { get; set; }
        public DateTime ShipDate { get; set; }
        public int OrdersId { get; set; }

        public virtual VmOrders Orders { get; set; }
        public virtual ICollection<VmRecyleOrderDetails> RecyleOrderDetails { get; set; }
    }
}
