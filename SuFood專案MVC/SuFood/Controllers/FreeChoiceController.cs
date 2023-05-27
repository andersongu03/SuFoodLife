using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
    public class FreeChoiceController : Controller
    {
        private readonly SuFoodDBContext _context;

        public FreeChoiceController(SuFoodDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FreeChoice()
        {
            return View();
        }

        [HttpGet]
        public async Task<IEnumerable<VmProductForFront>> GetProducts()
        {
            return _context.Products.Select(x => new VmProductForFront
            {
                ProductId= x.ProductId,
                ProductName= x.ProductName,
                Price= x.Price,
                Img = x.Img,
                Quantity = 0
            });
        }

        //GET: /FreeChoice/GetPlansAndProducts 取得方案與方案內的商品 !!有三張表的寫法
        public Object GetPlansAndProducts()
        {
            return _context.ProductsOfPlans.Where(p=> p.Plan.PlanStatus == true).GroupBy(p => p.PlanId).Select(group => new
            {
                plans = group.Select(p => new VmFreeChoicePlans
                {
                    PlanId = p.Plan.PlanId,
                    PlanName = p.Plan.PlanName,
                    PlanDescription = p.Plan.PlanDescription,
                    PlanPrice = p.Plan.PlanPrice,
                    PlanTotalCount = p.Plan.PlanTotalCount,
                    PlanCanChoiceCount = p.Plan.PlanCanChoiceCount,
                    PlanStatus = p.Plan.PlanStatus,
                }).SingleOrDefault(),
                Products = group.Select(p => new VmProductForFront
                {
                    ProductId = p.Product.ProductId,
                    ProductName = p.Product.ProductName,
                    Price = p.Product.Price,
                    Img = p.Product.Img,
                    Quantity = 0
                })
            });
        }
    }
}
