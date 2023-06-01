using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;
using System.Linq;

namespace SuFood.Controllers
{
    public class MyOrdersController : Controller
    {
        private readonly SuFoodDBContext _context;
        public MyOrdersController(SuFoodDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyOreders()
        {


            return View();
        }
        public async Task<JsonResult> Orders()
        {
            return Json(_context.Orders);
        }

        // MyOrders/GetMyOrders/(session的id)
        [HttpGet]
        public async Task<object> GetMyOrders(int getAccountId)
        {
            ICollection<VmComment> com = new List<VmComment>();
            int AccountId = -1;
            bool hasValue = int.TryParse(HttpContext.Session.GetString("GetAccountId"), out int res);
            if (hasValue)
            {
                AccountId = res;
            }

            List<VmMymodel> vmMymodels = new List<VmMymodel>();

            await _context.Orders.Include(x => x.OrdersReview).Where(o => o.AccountId == AccountId)
                .ForEachAsync(o =>
                {
                    vmMymodels.Add(new VmMymodel()
                    {
                        AccountId = AccountId,
                        Comments = o.OrdersReview.Select(x => x.Comment),
                        OrderStatus = o.OrderStatus,
                        SetOrdersDateTime = o.SetOrdersDatetime,
                        SubCost = o.SubCost,
                    });
                });

            return vmMymodels;
        }


        [HttpPost]
        public async Task<string> CreateComment([FromBody] VmComment x)
        {
            var exsist = _context.Orders.Where(o => o.OrdersId == x.OrdersId).Count();
            var Commented = _context.OrdersReview.Where(o => o.OrdersId == x.OrdersId).Count() == 0;
            if (exsist != 0 && Commented)
            {
                _context.OrdersReview.Add(new Models.OrdersReview()
                {
                    ReviewId = x.ReviewId,
                    Comment = x.Comment,
                    OrdersId = x.OrdersId,
                    RatingStar = x.RatingStar,
                });
                await _context.SaveChangesAsync();
                return "新增成功";

            }
            return "新增失敗";
        }

        [HttpPost]
        public async Task<string> EditComment([FromBody] OrdersReview Comment)
        {
            try
            {
                _context.OrdersReview.Select(or => new OrdersReview
                {
                    ReviewId = or.ReviewId,
                    RatingStar = or.RatingStar,
                    Comment = or.Comment,
                    OrdersId = or.OrdersId,
                });
                _context.Update(Comment);
                await _context.SaveChangesAsync();
                return " 修改成功";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderReviewExists(Comment.ReviewId))
                {
                    return "修改資料庫失敗";
                }
                else
                {
                    throw;
                }
            }
        }
        private bool OrderReviewExists(int id)
        {
            return (_context.OrdersReview?.Any(e => e.ReviewId == id)).GetValueOrDefault();
        }
    }
}
