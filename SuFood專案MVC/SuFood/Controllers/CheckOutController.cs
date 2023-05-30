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
	}
}
