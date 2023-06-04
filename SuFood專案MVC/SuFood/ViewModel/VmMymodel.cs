using SuFood.Models;

namespace SuFood.ViewModel
{
	public class VmMymodel
	{
		public int AccountId { get; set; }

		public DateTime? SetOrdersDateTime { get; set; }

		public string OrderStatus { get; set; }

		public int? SubCost { get; set; }

		public int? SubTotal { get; set; }

		public double? SubDiscount { get; set; }

		public IEnumerable<string>? Comments { get; set; }

	}
}
