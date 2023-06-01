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

        public object GetOrdersDetails2()
        {
            return _context.Orders.Select(o => new
            {
                OrdersId = o.OrdersId,
                SubTotal = o.SubTotal,
                SubCost = o.SubCost,
                ShipAddress = o.ShipAddress,
                CouponId = o.CouponId,
                OrdersDetails = o.OrdersDetails
            });
        }

        [HttpPost]
        public async Task<string> CreateOrders([FromBody] VmFreeChoicePlans vmParameters)
        {
            FreeChoicePlans fcp = new FreeChoicePlans
            {
                PlanId = vmParameters.PlanId,
                PlanName = vmParameters.PlanName,
                PlanDescription = vmParameters.PlanDescription,
                PlanPrice = vmParameters.PlanPrice,
                PlanCanChoiceCount = vmParameters.PlanCanChoiceCount,
                PlanTotalCount = vmParameters.PlanTotalCount,
                PlanStatus = vmParameters.PlanStatus,
                ProductsOfPlans = vmParameters.ProductsOfPlans,
            };

            _context.FreeChoicePlans.Add(fcp);
            await _context.SaveChangesAsync();
            return "新增成功";
        }

        // <示範> 生成自己功能頁面的Controller統一放在這裡。 例如: 優惠券管理頁面如下
        //public IActionResult Coupoun()
        //{
        //    return View();
        //}
    }
}

