//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SuFood.Models;
//using SuFood.ViewModel;

//namespace SuFood.Controllers
//{
//    public class ShoppingCartController : Controller
//    {
//        private readonly SuFoodDBContext _context;

//        public ShoppingCartController(SuFoodDBContext context)
//        {
//            _context = context;
//        }

//        //Get : ShoppingCartController/GetShoppingCart
//        [HttpGet]
//        public async Task<IEnumerable<ShoppingCartViewModel>> GetShoppingCarts()
//        {
//            var getAccountId = HttpContext.Session.GetString("GetAccountId");

//            var shoppingCarts = await _context.ShoppingCart
//                .Include(sc => sc.Product)
//                .ToListAsync();

//            var viewModelList = shoppingCarts
//                .Where(sc => sc.AccountId == Convert.ToInt32(getAccountId))
//                .Select(sc => new ShoppingCartViewModel
//                {
//                    CartId = sc.CartId,
//                    AccountId = sc.AccountId,
//                    CartQuantity = sc.CartQuantity,
//                    ProductId = sc.ProductId,
//                    ProductName = sc.Product.ProductName,
//                    Price = sc.Product.Price,
//                    Img = sc.Product.Img
//                }).ToList();

//            return viewModelList;
//        }
//        [HttpPut]
//        public async Task<IEnumerable<ShoppingCartViewModel>> AddCartItem([FromBody] ShoppingCartViewModel model)
//        {
//            var getAccountId = HttpContext.Session.GetString("GetAccountId");

//            var cartItem = await _context.ShoppingCart
//              .FirstOrDefaultAsync(c => c.ProductId == model.ProductId && c.AccountId == Convert.ToInt32(getAccountId));

//            if (cartItem == null)
//            {
//                return null;
//            }

//           cartItem.CartQuantity++;
//            _context.ShoppingCart.Update(cartItem);
//            await _context.SaveChangesAsync();

//            return (IEnumerable<ShoppingCartViewModel>)cartItem;
//        }


//        [HttpPost]
//        public async Task<IEnumerable<ShoppingCartViewModel>> MinusCartItem(ShoppingCartViewModel model)
//        {
//            var getAccountId = HttpContext.Session.GetString("GetAccountId");

//            var cartItem = await _context.ShoppingCart
//                .FirstOrDefaultAsync(c => c.ProductId == model.ProductId && c.AccountId == Convert.ToInt32(getAccountId));
                        

//            cartItem.CartQuantity--;

//            if (cartItem.CartQuantity == 0)
//            {
//                _context.ShoppingCart.Remove(cartItem);
//            }
//            else
//            {
//                _context.ShoppingCart.Update(cartItem);
//            }

//            await _context.SaveChangesAsync();

//            return (IEnumerable<ShoppingCartViewModel>)cartItem;
//        }
//        [HttpDelete]
//        public async Task<IEnumerable<ShoppingCartViewModel>> DeleteCartItem(ShoppingCartViewModel model)
//        {
//            var getAccountId = HttpContext.Session.GetString("GetAccountId");

//            var cartItem = await _context.ShoppingCart
//                .FirstOrDefaultAsync(c => c.ProductId == model.ProductId && c.AccountId == Convert.ToInt32(getAccountId));

           
//            _context.ShoppingCart.Remove(cartItem);

//            await _context.SaveChangesAsync();

//            return (IEnumerable<ShoppingCartViewModel>)cartItem;
//        }




//    }
//}
