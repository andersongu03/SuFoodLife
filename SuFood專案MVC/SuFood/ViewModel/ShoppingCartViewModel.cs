namespace SuFood.ViewModel
{
    public class ShoppingCartViewModel
    {
        public int CartId { get; set; }
        public int AccountId { get; set; }
        public int? CartQuantity { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Price { get; set; }
        public byte[] Img { get; set; }

    }
}
