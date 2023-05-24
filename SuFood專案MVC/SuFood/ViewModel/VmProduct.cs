namespace SuFood.ViewModel
{
    public class VmProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string StockUnit { get; set; }
        public int StockQuantity { get; set; }
        public int Price { get; set; }
        public int Cost { get; set; }
        public string Category { get; set; }
        public byte[] Img { get; set; }
        public int Quantity { get; set; }
    }
}
