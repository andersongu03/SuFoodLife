using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
    public class RetailController : Controller
    {
        private readonly SuFoodDBContext _context;

        public RetailController(SuFoodDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Retail()
        {
            return View();
        }
        public async Task<IActionResult> SingleProduct(int productId)
        {
            return View(productId);
        }
        [HttpGet]
        public async Task<IEnumerable<VmRetailList>> GetRetial()
        {
            var ProdcutsList = await _context.Products
                .Where(p => p.StockQuantity >= 100)
                .Select(p => new VmRetailList
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Category = p.Category,
                    Img = p.Img
                }).ToListAsync(); ;
            return ProdcutsList;
        }
        [HttpGet]
        public async Task<IEnumerable<VmRetailList>> SortProducts(string category)
        {
            var categoryProducts = await _context.Products
                .Where(p => p.Category == category && p.StockQuantity >= 100)
                .Select(p => new VmRetailList
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Img = p.Img,
                }).ToListAsync();


            return categoryProducts;
        }
        [HttpGet]
        public async Task<IEnumerable<VmRetailList>> GetCategories()
        {
            var GroupCategories = _context.Products
                 .Where(p => p.StockQuantity >= 100)
                .GroupBy(p => p.Category)
                .Select(p => new VmRetailList { Category = p.Key, CountCategory = p.Count() });

            return GroupCategories;
        }
        [HttpGet]
        public async Task<IEnumerable<SingleProductViewModel>> GetSingleProduct(int productId)
        {
            var SingleProducts = await _context.Products
                .Where(p => p.ProductId == productId)
                .Select(p => new SingleProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    StockUnit = p.StockUnit,
                    StockQuantity = p.StockQuantity,
                    Price = p.Price,
                    Img = p.Img
                })
                .ToListAsync();

            return SingleProducts;
        }
    }
}
