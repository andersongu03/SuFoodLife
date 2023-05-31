using SuFood.Models;

namespace SuFood.ViewModel
{
	public class VmMymodel
	{
		public int AccountId { get; set; }

		public DateTime? SetOrdersDateTime { get; set; }

		public string OrderStatus { get; set; }

		public int? SubCost { get; set; }

		public IEnumerable<string>? Comments { get; set; }

	}
}
