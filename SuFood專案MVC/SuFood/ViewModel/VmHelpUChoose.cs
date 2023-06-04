using SuFood.Models;

namespace SuFood.ViewModel
{
    public class VmHelpUChoose
    {
        public int HelpUchooseId { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }

        public virtual Products HelpUchooseNavigation { get; set; }
    }
}
