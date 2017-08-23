using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pizzeria.Data;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes
                .Include(b => b.Category)
                .Include(c => c.DishIngredients)
                .ThenInclude(e => e.Ingredient)
                .ToListAsync();

            return View(dishes);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult AddToCart(Dish dish, int? sessionId = null)
        {
            Basket basket;
            if (GetSessionString("basket") == null)
            {
                basket = new Basket();
                basket.Items.Add(new BasketItem()
                {
                    Dish = dish,
                    Quantity = 1
                });
            }
            else
            {
                basket = GetSessionObject("basket");

                if (basket.Items.Exists(basketItem => basketItem.DishId == dish.DishId))
                {
                    var existingItem = basket.Items.Single(x => x.DishId == dish.DishId);
                    existingItem.Quantity++;
                }
                else
                {
                    basket.Items.Add(new BasketItem()
                    {
                        Dish = dish,
                        Quantity = 1
                    });
                }
            }
            SaveToSession(basket);

            return View();
        }

        private Basket GetSessionObject(string sessionString)
        {
            return JsonConvert.DeserializeObject<Basket>(GetSessionString(sessionString));
        }

        private void SaveToSession(Basket basket)
        {
            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basket));
        }

        private string GetSessionString(string sessionName)
        {
            return HttpContext.Session.GetString(sessionName);
        }

        public IActionResult FilterCategories(int id)
        {
            var products = _context.Dishes
                .Where(x => x.CategoryId.Equals(id)).ToList();

            return View("Index", products);
        }
    }
}
