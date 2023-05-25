namespace SuFood.ViewModel
{
    public class SingleProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string StockUnit { get; set; }
        public int StockQuantity { get; set; }
        public int Price { get; set; }                
        public byte[] Img { get; set; }
    }
}
