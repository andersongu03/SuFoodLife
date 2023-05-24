using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")] //所有寫在Area裡面的Controller記得都要加這一行
    public class BackHomeController : Controller
    {
        private readonly SuFoodDBContext _context;

        public BackHomeController(SuFoodDBContext context)
        {
            _context = context;
        }

        //會員管理頁面
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

        // <示範> 生成自己功能頁面的Controller統一放在這裡。 例如: 優惠券管理頁面如下
        //public IActionResult Coupoun()
        //{
        //    return View();
        //}
    }
}

