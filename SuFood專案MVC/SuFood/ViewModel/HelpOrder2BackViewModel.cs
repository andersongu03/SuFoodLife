namespace SuFood.ViewModel
{
    public class HelpOrder2BackViewModel
    {
        public int TimeRange { get; set; }
        public int OrderFrequency { get; set; }
        public DateTime StartOrderDate { get; set; }
        public int OrderTotalPay { get; set; }
        public List<UserChioceProducts> UserChioce { get; set; }
    }
    public class UserChioceProducts
    {
        public string ProductName { get; set; }        
    }


}
