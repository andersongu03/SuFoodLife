using Microsoft.AspNetCore.Mvc;
using SuFood.Models;
using SuFood.ViewModel;
using System.Numerics;

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
		//[HttpGet]
		//public Object GetPlanToCart()
		//{
		//	return _context.ProductsOfPlans.GroupBy(p => p.PlanId).Select(group => new
		//	{
		//		Plan = group.Select(p => new VmPlanToCart
		//		{
		//			PlanId = p.Plan.PlanId,
		//			PlanName = p.Plan.PlanName,
		//			PlanDescription = p.Plan.PlanDescription,
		//			PlanPrice = p.Plan.PlanPrice,
		//			PlanTotalCount = p.Plan.PlanTotalCount,
		//			PlanCanChoiceCount = p.Plan.PlanCanChoiceCount,
		//			//select PlanId AS 訂單編號,
		//			//count(Product_Id) AS 幾筆Product
		//			//from ProductsOfPlans
		//			//group by PlanId
		//		}).FirstOrDefault(),
		//		Product = group.Select(p => new VmProductToCart
		//		{
		//			ProductId = p.Product.ProductId,
		//			ProductName = p.Product.ProductName,
		//			ProductDescription = p.Product.ProductDescription,
		//		})
		//	});
		//}

		//GET優惠券
		[HttpGet]
		public async Task<IEnumerable<VmCoupon>> GetCouponsToCart()
		{
			return _context.Coupon.Select(c => new VmCoupon
			{
				CouponId = c.CouponId,
				CouponName = c.CouponName,
				CouponDescription = c.CouponDescription,
				CouponMinusCost = c.CouponMinusCost,
				MinimumPurchasingAmount = c.MinimumPurchasingAmount,
				Couponstartdate2String = c.CouponStartDate.ToString().Substring(0,10),
				Couponenddate2String = c.CouponEndDate.ToString().Substring(0,10),
			});
		}

		//送出訂單
		[HttpPost]
		public async Task<string> Create([FromBody] VmSubmitOrder vmParameters)
		{
			Orders od = new Orders
			{
				OrdersId = vmParameters.OrdersId,
				SubTotal = vmParameters.SubTotal,
				SetOrdersDatetime = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond),
				ShipAddress = vmParameters.ShipAddress,
				CouponId = vmParameters.CouponId,
				OrdersDetails = vmParameters.OrdersDetails,
			};

			_context.Orders.Add(od);
			await _context.SaveChangesAsync();

			return "新增成功";
		}

	}
}
