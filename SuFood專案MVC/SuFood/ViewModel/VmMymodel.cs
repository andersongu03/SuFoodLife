using SuFood.Models;

namespace SuFood.ViewModel
{
	public class VmMymodel
	{
        public int OrdersId { get; set; }
        public int AccountId { get; set; }

		public DateTime? SetOrdersDateTime { get; set; }

		public string OrderStatus { get; set; }

		public int? SubCost { get; set; }

		public int? SubTotal { get; set; }

		public double? SubDiscount { get; set; }

		public string Phone { get; set; }
		public string Name { get; set; }

		public string ShipAddress { get; set; }
		public IEnumerable<string> Recomment { get; set; }

		public IEnumerable<string>? Comments { get; set; }

        public IEnumerable<VmRecyleSubscribeOrders> RecyleSubscribeOrders { get; set; }

        public  IEnumerable<RecyleOrderDetails>? RecyleOrderDetails { get; set; }

    }
	public class RecyleSubscribeOrders
	{
		public int ReSubOrdersId { get; set; }
		public DateTime ShipDate { get; set; }
        public string ShipStatus { get; set; }
    }
}
