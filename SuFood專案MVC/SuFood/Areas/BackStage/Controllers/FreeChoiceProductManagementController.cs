﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    public class FreeChoiceProductManagementController : Controller
    {
        private readonly SuFoodDBContext _context;

        public FreeChoiceProductManagementController(SuFoodDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FreeChoiceProduct()
        {
            return View();
        }

        //取得資料功能(1) GET: "/BackStage/FreeChoiceProductManagement/GetProducts"
        [HttpGet]
        public async Task<IEnumerable<VmProduct>> GetProducts()
        {
            return _context.Products.Select(pro => new VmProduct
            {
                ProductId = pro.ProductId,
                ProductName = pro.ProductName,
                ProductDescription = pro.ProductDescription,
                Price = pro.Price,
                Cost = pro.Cost,
                StockUnit = pro.StockUnit,
                StockQuantity = pro.StockQuantity,
                Category = pro.Category,
                Img = pro.Img,
                IsHelpUchioce = pro.IsHelpUchioce,                
            });
        }

        ////取得資料功能(2) GET: "/BackStage/FreeChoiceProductManagement/GetProductItem" 
        public async Task<JsonResult> GetProductItem()
        {
            return Json(_context.Products);
        }

        //刪除功能 DELETE: "/BackStage/FreeChoiceProductManagement/DeleteProducts/${id}"
        [HttpDelete]
        public async Task<string> DeleteProducts(int id)
        {
            if (_context.Products == null)
            {
                return "刪除失敗";
            }
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return "刪除失敗";
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return "刪除成功";
        }

        //新增功能
        [HttpPost]
        public async Task<string> Create([Bind("ProductId,ProductName,ProductDescription,StockUnit,StockQuantity,Price,Cost,Category,Img")] Products products)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files["Img"] != null)
                {
                    byte[] data = null; //準備data待會要放圖檔
                    using (BinaryReader br = new BinaryReader(Request.Form.Files["Img"].OpenReadStream()))
                    {
                        data = br.ReadBytes((int)Request.Form.Files["Img"].Length);
                        products.Img = data; //把檔案放回
                    }
                }

                _context.Products.Add(products);
                await _context.SaveChangesAsync();
                return "新增成功";
            }
            return "新增失敗";
        }

        //修改功能
        [HttpPost]
        public async Task<string> Edit([FromForm,Bind("ProductId,ProductName,ProductDescription,StockUnit,StockQuantity,Price,Cost,Category,Img")] Products products)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Form.Files["Img"] != null)
                    {
                        byte[] data = null; //準備data待會要放圖檔
                        using (BinaryReader br = new BinaryReader(Request.Form.Files["Img"].OpenReadStream()))
                        {
                            data = br.ReadBytes((int)Request.Form.Files["Img"].Length);
                            products.Img = data; //把檔案放回
                        }

                        
                    }
                    if (products.Img.Length < 500)
                    {
                        products.Img = _context.Products.Where(p => p.ProductId == products.ProductId).Select(p => p.Img).FirstOrDefault();
                    }


                    _context.Products.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
                    {
                        return "修改資料庫失敗";
                    }
                    else
                    {
                        throw;
                    }
                }
                return "修改成功";
            }
            return "修改失敗";
            }

        //修改幫你選商品清單功能
        //新增ProdcutOfPlans資料表
        [HttpPost]
        public async Task<string> EditHelpChoiceList([FromBody] List<VmProduct> vmParameters)
        {
            foreach (var vmParameter in vmParameters)
            {
                if (!_context.Products.Any(p => p.IsHelpUchioce == vmParameter.IsHelpUchioce))
                {
                    return "不存在該產品";
                }

                var product = _context.Products.Where(p => p.ProductId == vmParameter.ProductId);
                Products pro = new Products
                {
                    ProductId = vmParameter.ProductId,
                    IsHelpUchioce = vmParameter.IsHelpUchioce,
                    ProductName = vmParameter.ProductName,
                    Price= vmParameter.Price,
                    ProductDescription= vmParameter.ProductDescription,
                    Category = vmParameter.Category,
                    Cost= vmParameter.Cost,
                    Img= vmParameter.Img,
                    StockQuantity= vmParameter.StockQuantity,
                    StockUnit= vmParameter.StockUnit
                };

                _context.Products.Update(pro);
            }

            await _context.SaveChangesAsync();

            return "新增成功";
        }


        private bool ProductsExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
