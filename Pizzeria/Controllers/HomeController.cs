using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

            ViewBag.HideSection = false;

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
        public IActionResult AddToCart(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(x => x.DishId == dishId);

            Basket basket;

            if (HttpContext.Session.GetInt32("BasketId") == null)
            {
                basket = new Basket();
                basket.Items = new List<BasketItem>
                {
                    new BasketItem()
                    {
                        Dish = dish,
                        Quantity = 1
                    }
                };
            }
            else
            {
                var basketId = HttpContext.Session.GetInt32("BasketId");
                basket = _context.Baskets.FirstOrDefault(x => x.BasketId == basketId);

                if (basket.Items != null && basket.Items.Exists(basketItem => basketItem.DishId == dish.DishId))
                {
                    var existingItem = basket.Items.Single(x => x.DishId == dish.DishId);
                    existingItem.Quantity++;
                }
                else
                {
                    basket.Items?.Add(new BasketItem()
                    {
                        Dish = dish,
                        Quantity = 1
                    });
                }
            }
            SaveBasket(basket);

            return View("Index", _context.Dishes);
        }

        public Basket GetSessionObject(string sessionString)
        {
            return JsonConvert.DeserializeObject<Basket>(GetSessionString(sessionString));
        }

        private void SaveBasket(Basket basket)
        {
            _context.Baskets.Add(basket);
            _context.SaveChanges();
        }

        public string GetSessionString(string sessionName)
        {
            return HttpContext.Session.GetString(sessionName);
        }

        public IActionResult FilterCategories(int id)
        {
            var products = _context.Dishes
                .Where(x => x.CategoryId.Equals(id))
                .Include(b => b.Category)
                .Include(c => c.DishIngredients)
                .ThenInclude(e => e.Ingredient)
                .ToList();

            return View("Index", products);
        }
    }
}
