using SuFood.Models;

namespace SuFood.ViewModel
{
    public class VmFreeChoicePlans
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public int? PlanTotalCount { get; set; }
        public int? PlanCanChoiceCount { get; set; }
        public int? PlanPrice { get; set; }
        public bool? PlanStatus { get; set; }

        public virtual ICollection<ProductsOfPlans> ProductsOfPlans { get; set; }
    }
}
