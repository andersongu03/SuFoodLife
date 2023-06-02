using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;
using System.Net;
using System.Numerics;
using System.Security.Policy;
using System.Xml.Linq;

namespace SuFood.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly SuFoodDBContext _context;
        public CheckOutController(SuFoodDBContext context)
        {
            _context = context;
        }
        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: /CheckOut/GetPlanToCart 
        [HttpGet]
        public Object GetPlanToCart()
        {
            return _context.ProductsOfPlans.GroupBy(p => p.PlanId).Select(group => new
            {
                Plan = group.Select(p => new VmPlanToCart
                {
                    PlanId = p.Plan.PlanId,
                    PlanName = p.Plan.PlanName,
                    PlanDescription = p.Plan.PlanDescription,
                    PlanPrice = p.Plan.PlanPrice,
                    PlanTotalCount = p.Plan.PlanTotalCount,
                    PlanCanChoiceCount = p.Plan.PlanCanChoiceCount,
                    //select PlanId AS 訂單編號,
                    //count(Product_Id) AS 幾筆Product
                    //from ProductsOfPlans
                    //group by PlanId
                }).FirstOrDefault(),
                Product = group.Select(p => new VmProductToCart
                {
                    ProductId = p.Product.ProductId,
                    ProductName = p.Product.ProductName,
                    ProductDescription = p.Product.ProductDescription,
                })
            }); ;
        }

        //送出訂單
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VmSubmitOrder vmParameters)
        {
            Orders od = new Orders
            {
                OrdersId = vmParameters.OrdersId,
                SubTotal = vmParameters.SubTotal,
                SetOrdersDatetime = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond),
                ShipAddress = vmParameters.ShipAddress,
                CouponId = vmParameters.CouponId,
                OrdersDetails = vmParameters.OrdersDetails,
                AccountId = vmParameters.AccountId
            };

            _context.Orders.Add(od);
            await _context.SaveChangesAsync();

            var id = od.OrdersId;
            return Json(new { OrderId = id });
        }

        [HttpGet]
        public async Task<string> GetCurrentAccountId()
        {
            return HttpContext.Session.GetString("GetAccountId");
        }
        //==============================以上阿信的==================================
        public IActionResult Retail2Order()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRetailOrder([FromBody] RetailOrdersViewModel model)
        {
            var getAccountId = HttpContext.Session.GetString("GetAccountId");

            var existingOrderId = _context.Orders.Where(o => o.AccountId == Convert.ToInt32(getAccountId) && o.OrderStatus == "未付款").Select(o => o.OrdersId).First();

            if (existingOrderId != 0)
            {
                return await EditRetailOrder(existingOrderId, model);
            }

            var getSubCost = _context.Products.Where(p => p.ProductName == model.ProductName).Sum(p => p.Cost);

            if (ModelState.IsValid)
            {
                var order = new Orders
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    ShipAddress = model.ShipAddress,
                    SubTotal = model.SubTotal,
                    SubCost = getSubCost,
                    SubDiscount = model.SubDiscount,
                    SetOrdersDatetime = DateTime.Now,
                    AccountId = Convert.ToInt32(getAccountId),
                    OrderStatus = "未付款",
                    Email = model.Email
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                var getNewOrderId = order.OrdersId;

                var orderDetails = new OrdersDetails
                {
                    OrderId = getNewOrderId,
                    ProductName = model.ProductName,
                    UnitPrice = model.UnitPrice,
                    Quantity = model.Quantity,
                };
                _context.OrdersDetails.Add(orderDetails);
                await _context.SaveChangesAsync();
                return Json(new { GetOrderId = getNewOrderId });
            }
            return Ok("我怎麼在這");
        }
        [HttpPost]
        public async Task<IActionResult> EditRetailOrder(int orderId, RetailOrdersViewModel model)
        {
            var order = await _context.Orders.FindAsync(orderId);
            var getSubCost = _context.Products.Where(p => p.ProductName == model.ProductName).Sum(p => p.Cost);
            if (order == null) { return Ok("我是誰我在哪"); }

            order.Name = model.Name;
            order.Phone = model.Phone;
            order.ShipAddress = model.ShipAddress;
            order.SubCost = getSubCost;
            order.SubTotal = model.SubTotal;
            order.SubDiscount = model.SubDiscount;
            order.SetOrdersDatetime = DateTime.Now;
            order.ShipAddress = model.ShipAddress;

            

            var orderDetails = _context.OrdersDetails.Where(od => od.OrderId == orderId);
            _context.OrdersDetails.RemoveRange(orderDetails);

            var NewOrderDetails = new OrdersDetails
            {
                OrderId = orderId,
                ProductName = model.ProductName,
                UnitPrice = model.UnitPrice,
                Quantity = model.Quantity,
            };

            _context.OrdersDetails.Add(NewOrderDetails);
            await _context.SaveChangesAsync();

            return Json(new { GetOrderId = orderId });
        }

    }

}

