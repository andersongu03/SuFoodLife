﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
    public class ShoppingCartController
    {
        private readonly SuFoodDBContext _context;

        public ShoppingCartController(SuFoodDBContext context)
        {
            _context = context;
        }

        //Get : ShoppingCartController/GetShoppingCart
        [HttpGet]
        public async Task<IEnumerable<ShoppingCartViewModel>> GetShoppingCarts()
        {
            var shoppingCarts = await _context.ShoppingCart
                .Include(sc => sc.Product)
                .ToListAsync();

            var viewModelList = shoppingCarts.Select(sc => new ShoppingCartViewModel
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
        //Post :

    }
}
