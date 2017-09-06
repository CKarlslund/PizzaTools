using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pizzeria.Data;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdersController> _logger;     

        public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Order.Include(o => o.Basket);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Basket)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["BasketId"] = new SelectList(_context.Baskets, "BasketId", "BasketId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Paid,Delivered,Finished,OrderDate,ApplicationUserId,BasketId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BasketId"] = new SelectList(_context.Baskets, "BasketId", "BasketId", order.Basket.BasketId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["BasketId"] = new SelectList(_context.Baskets, "BasketId", "BasketId", order.Basket.BasketId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Paid,Delivered,Finished,OrderDate,ApplicationUserId,BasketId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["BasketId"] = new SelectList(_context.Baskets, "BasketId", "BasketId", order.Basket.BasketId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Basket)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.SingleOrDefaultAsync(m => m.OrderId == id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

        public IActionResult Payment(CheckoutInfo checkoutInfo)
        {
            //var checkoutInfo = _context.CheckoutInfo;

            var order = _context.Order
                .Include(o => o.Basket)
                .FirstOrDefault(x => x.OrderId == checkoutInfo.OrderId);

            var basket = _context.Baskets
                .Include(b => b.Items)
                .ThenInclude(c => c.Dish)
                .FirstOrDefault(x => x.BasketId == order.Basket.BasketId);

            //Calculate total
            var total = 0;

            foreach (var basketItem in basket.Items)
            {
                total += (basketItem.Quantity * basketItem.Dish.Price);
            }

            var shipping = 0;

            if (basket.Items.Count < 5)
            {
                shipping = 49;
            }
            
            total += shipping;

            order.Total = total;
            order.Shipping = shipping;
            _context.SaveChanges();

            return View(checkoutInfo);
        }

        public async Task<IActionResult> LoginOrAnonymous()
        {
            var basketId = HttpContext.Session.GetInt32("BasketId");
            var basket = _context.Baskets
                .Include(x => x.Items)
                .ThenInclude(y => y.BasketItemIngredients)
                .Include(z => z.Items)
                .ThenInclude(h => h.Dish)
                .ThenInclude(j => j.DishIngredients)
                .ThenInclude(k => k.Ingredient)
                .FirstOrDefault(x => x.BasketId == basketId);

            var order = new Order
            {
                Basket = basket
            };

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            var checkoutInfo = new CheckoutInfo {OrderId = order.OrderId};

            _context.CheckoutInfo.Add(checkoutInfo);
            await _context.SaveChangesAsync();

            return View(checkoutInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutInfo([Bind("Id, OrderId,FirstName,LastName,Email,PostingAddress,PostalCode,City,PhoneNumber")]CheckoutInfo checkoutInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.CheckoutInfo.Update(checkoutInfo);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Payment", checkoutInfo);
                }
                catch (DbUpdateConcurrencyException exc)
                {                   
                    _logger.LogError(exc.ToString());
                }
                return RedirectToAction(nameof(Index));
            }
            return View("LoginOrAnonymous", checkoutInfo);                        
        }
    }
}
