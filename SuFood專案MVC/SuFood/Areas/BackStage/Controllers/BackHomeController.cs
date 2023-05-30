using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;
using System.Net;

namespace SuFood.Areas.BackStage.Controllers
{
    [AllowAnonymous]
    [Area("BackStage")] //所有寫在Area裡面的Controller記得都要加這一行
    public class BackHomeController : Controller
    {
        private readonly SuFoodDBContext _context;
        public BackHomeController(SuFoodDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //自由選商品管理頁面
        public IActionResult FreeChoiceProduct()
        {
            return View();
        }

        public IActionResult Coupon()
        {
            return View();
        }


        //會員管理頁面
        public IActionResult AccountManage()
        {
            return View();
        }

        //訂單管理頁面
        public IActionResult OrdersManage()
        {
            return View();
        }

        public IActionResult FreeChoicePlans()
        {
            return View();
        }

        public IActionResult CommentManagement()

        {
            return View();
        }

        public object GetOrdersDetails()
        {
            return _context.OrdersDetails.GroupBy(od => od.Order.OrdersId).Select(o => new
            {
                OrderId = o.Select(o => o.OrderId).SingleOrDefault()
            });
        }

        public object GetOrdersDetails2()
        {
            var orderId = _context.Orders.Where(x => x.OrdersId == 2).Select(d => d.OrdersId).FirstOrDefault();
            var productIdofOrderDetailds = _context.OrdersDetails.Where(od => od.OrderId == orderId);
            return productIdofOrderDetailds;
        }

        // <示範> 生成自己功能頁面的Controller統一放在這裡。 例如: 優惠券管理頁面如下
        //public IActionResult Coupoun()
        //{
        //    return View();
        //}
    }
}

