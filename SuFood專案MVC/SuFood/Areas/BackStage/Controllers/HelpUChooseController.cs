using Microsoft.AspNetCore.Mvc;
using SuFood.ViewModel;
using SuFood.Models;
using Microsoft.EntityFrameworkCore;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    public class HelpUChooseController : Controller
    {
        private readonly SuFoodDBContext _context;

        public HelpUChooseController(SuFoodDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IEnumerable<VmHelpUChoose>> GetData()
        {
            return _context.HelpUchoose.Select(huc => new VmHelpUChoose
            {
                HelpUchooseId=huc.HelpUchooseId,
                ProductId=huc.ProductId,
                Price = huc.Price,
            });
        }
    }
}
