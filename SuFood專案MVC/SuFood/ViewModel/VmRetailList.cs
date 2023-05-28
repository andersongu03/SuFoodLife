namespace SuFood.ViewModel
{
    public class VmRetailList
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public byte[] Img { get; set; }
        public string Category { get; set; }
        public int CountCategory { get; set; }
        public int StockQuantity { get; set; }
    }
}
