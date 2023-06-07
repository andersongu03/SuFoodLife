using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;

namespace SuFood.Areas.BackStage.Controllers
{
	[Area("BackStage")]
	public class OrdersManagementController : Controller
	{
		private readonly SuFoodDBContext _context;

		public OrdersManagementController(SuFoodDBContext context)
		{
			_context = context;
		}

		// GET: BackStage/OrdersManagement
		public async Task<IActionResult> Index()
		{
			var suFoodDBContext = _context.Orders.Include(o => o.Account).Include(o => o.Coupon);
			return View(await suFoodDBContext.ToListAsync());
		}

		// GET: BackStage/OrdersManagement/Details/5
		//public async Task<IActionResult> Details(int? id)
		//{
		//    if (id == null || _context.Orders == null)
		//    {
		//        return NotFound();
		//    }

		//    var orders = await _context.Orders
		//        .Include(o => o.Account)
		//        .Include(o => o.Coupon)
		//        .FirstOrDefaultAsync(m => m.OrdersId == id);
		//    if (orders == null)
		//    {
		//        return NotFound();
		//    }

		//    return View(orders);
		//}

		public object getAllOrders() 
		{
			return _context.Orders.Include(x => x.Account).Include(x => x.OrdersDetails).Select(x => new
			{
				or = new
				{ 
					accountId = x.AccountId,
					account = x.Account.Account1, 
					orderTime = x.SetOrdersDatetime,
					status = x.OrderStatus,
					shipMode = x.SingleShippingMethod,					
					subTotal = x.SubTotal
				}

			}).ToList();
		}



	 

		// GET: BackStage/OrdersManagement/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Orders == null)
			{
				return NotFound();
			}

			var orders = await _context.Orders.FindAsync(id);
			if (orders == null)
			{
				return NotFound();
			}
			ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "AccountId", orders.AccountId);
			ViewData["CouponId"] = new SelectList(_context.Coupon, "CouponId", "CouponId", orders.CouponId);
			return View(orders);
		}

		// POST: BackStage/OrdersManagement/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("OrdersId,SubTotal,SubCost,SubDiscount,SetOrdersDatetime,ShipAddress,OrderStatus,ShippingMethodId,AccountId,CouponId,OrdersDetailsId,CustomerPaymentId")] Orders orders)
		{
			if (id != orders.OrdersId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(orders);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!OrdersExists(orders.OrdersId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "AccountId", orders.AccountId);
			ViewData["CouponId"] = new SelectList(_context.Coupon, "CouponId", "CouponId", orders.CouponId);
			return View(orders);
		}

		// GET: BackStage/OrdersManagement/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Orders == null)
			{
				return NotFound();
			}

			var orders = await _context.Orders
				.Include(o => o.Account)
				.Include(o => o.Coupon)
				.FirstOrDefaultAsync(m => m.OrdersId == id);
			if (orders == null)
			{
				return NotFound();
			}

			return View(orders);
		}

		// POST: BackStage/OrdersManagement/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Orders == null)
			{
				return Problem("Entity set 'SuFoodDBContext.Orders'  is null.");
			}
			var orders = await _context.Orders.FindAsync(id);
			if (orders != null)
			{
				_context.Orders.Remove(orders);
			}
			
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool OrdersExists(int id)
		{
		  return (_context.Orders?.Any(e => e.OrdersId == id)).GetValueOrDefault();
		}
	}
}
