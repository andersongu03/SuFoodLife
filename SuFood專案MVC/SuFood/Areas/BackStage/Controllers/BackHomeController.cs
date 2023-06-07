using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;
using System.Net;

namespace SuFood.Areas.BackStage.Controllers
{
    [Authorize(Roles = "Employee")]
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
        {// 檢查使用者是否已驗證
            return View();
        }

        //訂單管理頁面
        public IActionResult OrdersManage()
        {// 檢查使用者是否已驗證
            return View();
        }

        public IActionResult FreeChoicePlans()
        {// 檢查使用者是否已驗證
            return View();
        }

        public IActionResult CommentManagement()
        {// 檢查使用者是否已驗證
            return View();
        }

        public IActionResult BackStageChat()
        {// 檢查使用者是否已驗證
            return View();
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

        [HttpGet]
        public object GetOrdersDetails()
        {
            return _context.OrdersDetails.Where(od=> od.OrderId == 1).Select(od => new
            {
                ProductName = od.ProductName,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                CouponName = _context.Coupon.Where(c => c.CouponId == od.CouponId).FirstOrDefault().CouponName,
                CouponDicount = _context.Coupon.Where(c => c.CouponId == od.CouponId).FirstOrDefault().CouponMinusCost
            });
        }


        // <示範> 生成自己功能頁面的Controller統一放在這裡。 例如: 優惠券管理頁面如下
        //public IActionResult Coupoun()
        //{
        //    return View();
        //}
    }
}

