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

            await _context.Orders.Include(od => od.OrdersDetails).Include(x => x.OrdersReview).Include(rc => rc.RecyleSubscribeOrders).ThenInclude(d=>d.RecyleOrderDetails).Where(o => o.AccountId == AccountId)
                .ForEachAsync(o =>
                {
                    vmMymodels.Add(new VmMymodel()
                    {
                        AccountId = AccountId,
                        OrdersId = o.OrdersId,
                        OrdersDetails = o.OrdersDetails.Select(od => new VmOrdersDetails()
                        {
                            ProductName = od.ProductName,
                            Quantity = od.Quantity,
                        }),
                        Comments = o.OrdersReview.Select(x => x.Comment),
                        OrderStatus = o.OrderStatus,
                        SetOrdersDateTime = o.SetOrdersDatetime,
                        SubTotal = o.SubTotal,
                        SubCost = o.SubCost,
                        SubDiscount = o.SubDiscount,
                        Phone = o.Phone,
                        Name = o.Name,
                        ShipAddress = o.ShipAddress,
                        Recomment = o.OrdersReview.Select(r => r.Recomment),
                        RecyleSubscribeOrders = o.RecyleSubscribeOrders.Select(rs => new VmRecyleSubscribeOrders()
                        {
                            ReSubOrdersId = rs.ReSubOrdersId,
                            ShipDate = rs.ShipDate,
                            ShipStatus = rs.ShipStatus,
                            RecyleOrderDetails = rs.RecyleOrderDetails.Select(rod => new VmRecyleOrderDetails()
                            {
                                RecyleOrderDetailsId = rod.RecyleOrderDetailsId,
                                ProductName = rod.ProductName,
                                Quantity = rod.Quantity
                            }),
                        }),
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
                OrdersReview or = new OrdersReview()
                {
                    OrdersId = x.OrdersId,
                    RatingStar = x.RatingStar,
                    Comment = x.Comment,
                };

                _context.OrdersReview.Add(or);
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
