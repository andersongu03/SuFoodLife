using SuFood.Models;

namespace SuFood.ViewModel
{
    public class VmComment
    {
        public int ReviewId { get; set; }
        public int? RatingStar { get; set; }
        public string Comment { get; set; }
        public int OrdersId { get; set; }

		public int? AccountId { get; set; }

		public string Phone { get; set; }

		public ICollection<OrdersReview> OrdersReviews { get; set; }
	}
}
