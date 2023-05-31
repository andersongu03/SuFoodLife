using SuFood.Models;

namespace SuFood.ViewModel
{
    public class CartViewModels
    {        
        public int CartId { get; set; }
        public int AccountId { get; set; }
        public int? CartQuantity { get; set; }
        public int ProductId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Products Product { get; set; }
    }
}
