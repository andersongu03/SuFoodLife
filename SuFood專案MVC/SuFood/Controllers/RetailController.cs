using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        //[httppost]
        //public async task<ienumerable<singleproductviewmodel>> getsingleproduct(int productid)
        //{
        //var singleproducts = await _context.products
        //    .where(p => p.productid == productid)
        //    .select(p => new singleproductviewmodel
        //    {
        //        productid = p.productid,
        //        productname = p.productname,
        //        productdescription = p.productdescription,
        //        stockunit = p.stockunit,
        //        stockquantity = p.stockquantity,
        //        price = p.price,
        //        img = p.img
        //    })
        //    .tolistasync();

        //    return singleproducts;
        //}

        [HttpGet]
        public async Task<SingleProductViewModel> GetSingleProduct(int productId)
        {
            var p = await _context.Products.FindAsync(productId);

            SingleProductViewModel result = new SingleProductViewModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Img = p.Img,
                ProductDescription = p.ProductDescription,
                StockQuantity = p.StockQuantity,
            };

            return result;
        }
    }
}
