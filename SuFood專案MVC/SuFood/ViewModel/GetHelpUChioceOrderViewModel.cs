namespace SuFood.ViewModel
{
    public class GetHelpUChioceOrderViewModel
    {
        public int OrdersId { get; set; }
        public int SubTotal { get; set; }
        public int countShip { get; set; }
        public List<RecyleOrderDetailsViewModel> DetailsViewModels { get; set; }
    }
    public class RecyleOrderDetailsViewModel
    {
        public DateTime ShipDate { get; set; }
    }
}
