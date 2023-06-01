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

        [HttpPost]
        public async Task<IActionResult> Add2Cart([FromBody] CartViewModels model)
        {
            var getAccountId = HttpContext.Session.GetString("GetAccountId");

            if (getAccountId == null)
            {
                return Json(new { GetAccountId = "null" });
            }

            var existingCart = await _context.ShoppingCart
        .FirstOrDefaultAsync(c => c.ProductId == model.ProductId && c.AccountId == Convert.ToInt32(getAccountId));

            if (existingCart != null)
            {
                existingCart.CartQuantity++;
                await _context.SaveChangesAsync();
            }
            else
            {
                ShoppingCart cart = new ShoppingCart
                {
                    CartId = model.CartId,
                    AccountId = Convert.ToInt32(getAccountId),
                    CartQuantity = 1,
                    ProductId = model.ProductId,
                };
                _context.ShoppingCart.Add(cart);
                await _context.SaveChangesAsync();
            }

            return Json(new { GetAccountId = getAccountId });
        }

        [HttpGet]
        public async Task<IEnumerable<ShoppingCartViewModel>> GetShoppingCarts()
        {
            var getAccountId = HttpContext.Session.GetString("GetAccountId");

            var shoppingCarts = await _context.ShoppingCart
                .Include(sc => sc.Product)
                .ToListAsync();

            var viewModelList = shoppingCarts
                .Where(sc => sc.AccountId == Convert.ToInt32(getAccountId))
                .Select(sc => new ShoppingCartViewModel
                {
                    CartId = sc.CartId,
                    AccountId = sc.AccountId,
                    CartQuantity = sc.CartQuantity,
                    ProductId = sc.ProductId,
                    ProductName = sc.Product.ProductName,
                    Price = sc.Product.Price,
                    Img = sc.Product.Img
                }).ToList();

            return viewModelList;
        }
        [HttpPost]
        public async Task<bool> AddCartItem([FromBody] ShoppingCartViewModel model)
        {
            var getAccountId = HttpContext.Session.GetString("GetAccountId");

            var cartItem = await _context.ShoppingCart
              .FirstOrDefaultAsync(c => c.ProductId == model.ProductId && c.AccountId == Convert.ToInt32(getAccountId));

            if (cartItem == null)
            {
                return false;
            }

            cartItem.CartQuantity++;
            _context.ShoppingCart.Update(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }


        [HttpPost]
        public async Task<bool> MinusCartItem([FromBody] ShoppingCartViewModel model)
        {
            var getAccountId = HttpContext.Session.GetString("GetAccountId");

            var cartItem = await _context.ShoppingCart
                .FirstOrDefaultAsync(c => c.ProductId == model.ProductId && c.AccountId == Convert.ToInt32(getAccountId));

            if (cartItem == null)
            {
                return false;
            }
            cartItem.CartQuantity--;

            if (cartItem.CartQuantity == 0)
            {
                _context.ShoppingCart.Remove(cartItem);
            }
            else
            {
                _context.ShoppingCart.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            await GetShoppingCarts();
            return true;
        }
        [HttpPost]
        public async Task<bool> DeleteCartItem([FromBody] ShoppingCartViewModel model)
        {
            var getAccountId = HttpContext.Session.GetString("GetAccountId");

            var cartItem = await _context.ShoppingCart
                .FirstOrDefaultAsync(c => c.ProductId == model.ProductId && c.AccountId == Convert.ToInt32(getAccountId));
            if (cartItem == null)
            {
                return false;
            }

            _context.ShoppingCart.Remove(cartItem);

            await _context.SaveChangesAsync();
            await GetShoppingCarts();
            return true;
        }

    }
}
