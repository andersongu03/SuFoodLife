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
    }
}
