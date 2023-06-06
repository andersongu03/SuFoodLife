using SuFood.Models;

namespace SuFood.ViewModel
{
	public class VmCustomerPayment
	{
		public int CustomerPaymentId { get; set; }
		public string CreditCardHolder { get; set; }
		public string CreditCardNumber { get; set; }
		public string CreditCardExpiryDate { get; set; }
		public int? OrdersId { get; set; }

		public virtual Orders Orders { get; set; }
	}
}
